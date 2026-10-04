using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Service.Options;
using ServiceAbstraction;
using ServiceAbstraction.Attachments;

namespace Service.Attachments
{
    /// <summary>
    /// The attachment decisions that every service whose entity carries a stored file has to make,
    /// written once against <see cref="IAttachmentService"/> so the <c>IsDelete</c> contract and the
    /// "relative paths only" rule cannot drift apart between services.
    /// <para>
    /// These are extension methods, not a base class: each service takes <see cref="IAttachmentService"/>
    /// through its constructor and performs its own upload and delete calls. Nothing here replaces
    /// that dependency, it only keeps the rules around those calls in one place.
    /// </para>
    /// </summary>
    public static class AttachmentServiceExtensions
    {
        /// <summary>
        /// Stores a new entity's attachment and commits it, undoing the upload if anything in
        /// between fails. This and <see cref="UpdateAttachmentAsync"/> are the only supported way
        /// to move an attachment, because they hold the whole span from the upload to the commit.
        /// Calling <c>ResolveAttachmentAsync</c> and <c>CommitAttachmentChangeAsync</c> yourself
        /// reopens the gap between them: an exception raised while the entity is being prepared
        /// would leave the freshly uploaded file on disk with no row pointing at it, and nothing
        /// would remove it. Those two are private for that reason.
        /// <para>
        /// Pass <paramref name="additionalWork"/> when the service still has database work to do
        /// for the same request. It runs inside the commit's transaction, so that work and the
        /// attachment become visible together or not at all.
        /// </para>
        /// </summary>
        public static Task<int> CreateAttachmentAsync<TEntity, TKey>(
            this IUnitOfWork unitOfWork,
            IGenericRepository<TEntity, TKey> repository,
            IAttachmentService attachmentService,
            AttachmentOptions attachmentOptions,
            TEntity entity,
            string? storedPath,
            IFormFile? file,
            string folderKey,
            bool isDelete,
            Action<TEntity, AttachmentResolution> apply,
            Func<Task>? additionalWork = null)
            where TEntity : BaseEntity<TKey>
            => ApplyAttachmentAsync(unitOfWork, repository, attachmentService, attachmentOptions,
                entity, storedPath, file, folderKey, isDelete, apply, isNew: true, additionalWork);

        /// <summary>
        /// Replaces an existing entity's attachment and commits it, undoing the upload if anything
        /// in between fails. See <see cref="CreateAttachmentAsync"/> for why this is the entry
        /// point rather than a call to the upload and commit steps individually.
        /// </summary>
        public static Task<int> UpdateAttachmentAsync<TEntity, TKey>(
            this IUnitOfWork unitOfWork,
            IGenericRepository<TEntity, TKey> repository,
            IAttachmentService attachmentService,
            AttachmentOptions attachmentOptions,
            TEntity entity,
            string? storedPath,
            IFormFile? file,
            string folderKey,
            bool isDelete,
            Action<TEntity, AttachmentResolution> apply,
            Func<Task>? additionalWork = null)
            where TEntity : BaseEntity<TKey>
            => ApplyAttachmentAsync(unitOfWork, repository, attachmentService, attachmentOptions,
                entity, storedPath, file, folderKey, isDelete, apply, isNew: false, additionalWork);

        /// <summary>
        /// The single guarded span: upload, hand the result to the entity, stage it, commit. The
        /// handover matters. Before the commit is entered nothing references the new file, so a
        /// failure in <c>apply</c> or in staging has to remove the upload to avoid orphaning it.
        /// Once the commit has been entered it owns that cleanup itself, and doing it twice would
        /// only add a redundant delete.
        /// </summary>
        private static async Task<int> ApplyAttachmentAsync<TEntity, TKey>(
            IUnitOfWork unitOfWork,
            IGenericRepository<TEntity, TKey> repository,
            IAttachmentService attachmentService,
            AttachmentOptions attachmentOptions,
            TEntity entity,
            string? storedPath,
            IFormFile? file,
            string folderKey,
            bool isDelete,
            Action<TEntity, AttachmentResolution> apply,
            bool isNew,
            Func<Task>? additionalWork)
            where TEntity : BaseEntity<TKey>
        {
            var resolution = await attachmentService.ResolveAttachmentAsync(
                attachmentOptions, storedPath, file, folderKey, isDelete);

            var handedOver = false;

            try
            {
                apply(entity, resolution);

                if (isNew)
                    await repository.AddAsync(entity);
                else
                    repository.Update(entity);

                handedOver = true;

                return await unitOfWork.CommitAttachmentChangeAsync(
                    attachmentService, resolution, additionalWork);
            }
            catch
            {
                if (!handedOver)
                    DiscardUploadedFile(attachmentService, resolution);

                throw;
            }
        }

        /// <summary>
        /// Uploads the incoming file and works out the path the entity should carry from now on.
        /// Nothing already on disk is touched here: a file that is being replaced or removed is
        /// reported back as <see cref="AttachmentResolution.ReplacedPath"/> and retired by
        /// <see cref="CommitAttachmentChangeAsync"/>, once the row that stops referencing it has
        /// actually been written.
        /// <list type="bullet">
        /// <item>Delete intent with a replacement: the new file is stored and the old one is reported for retirement.</item>
        /// <item>Delete intent alone: the path is cleared and the old file is reported for retirement.</item>
        /// <item>Replacement alone: the new file is stored and the old one is reported for retirement.</item>
        /// <item>Neither: the stored attachment is left exactly as it was.</item>
        /// </list>
        /// If the upload fails this throws before returning, so the caller never assigns a path and
        /// the existing attachment stays in place.
        /// The returned <see cref="AttachmentResolution.StoredPath"/> is already in the form the
        /// database holds, so it can be assigned to the entity as-is.
        /// </summary>
        private static async Task<AttachmentResolution> ResolveAttachmentAsync(
            this IAttachmentService attachmentService,
            AttachmentOptions attachmentOptions,
            string? storedPath,
            IFormFile? file,
            string folderKey,
            bool isDelete)
        {
            if (file is null)
                return new AttachmentResolution
                {
                    StoredPath = isDelete ? string.Empty : attachmentOptions.ToStoredPath(storedPath),
                    ReplacedPath = isDelete ? storedPath : null
                };

            var uploadedPath = await attachmentService.Upload(file, attachmentOptions.GetFolder(folderKey));

            if (string.IsNullOrEmpty(uploadedPath))
                throw new AttachmentException($"The {folderKey} attachment could not be stored.");

            var newPath = attachmentOptions.ToStoredPath(uploadedPath);

            return new AttachmentResolution
            {
                StoredPath = newPath,
                StoredNewFile = true,
                FileName = Path.GetFileName(file.FileName),
                FileType = file.ContentType,

                // A replacement retires whatever was there before, including when the request also
                // asked for the old attachment to be deleted.
                ReplacedPath = storedPath != newPath ? storedPath : null
            };
        }

        /// <summary>
        /// Writes the row and then settles the disk to match it, in that order: on success the file
        /// the row no longer references is removed; if the write did not land, or threw, the freshly
        /// uploaded file is removed instead and the previous one is left untouched, so a failed
        /// request can neither lose the old attachment nor orphan the new.
        /// <para>
        /// Pass <paramref name="additionalWork"/> when the service still has database work to do for
        /// the same request, such as replacing a skill's types or a project's categories. That work
        /// runs inside the same transaction as the attachment write, so the attachment and the rest
        /// of the request become visible together or not at all: a failure in a later save can no
        /// longer leave the new attachment committed while the request as a whole has failed. The
        /// replaced file is still removed only once the transaction has committed.
        /// </para>
        /// </summary>
        private static async Task<int> CommitAttachmentChangeAsync(
            this IUnitOfWork unitOfWork,
            IAttachmentService attachmentService,
            AttachmentResolution resolution,
            Func<Task>? additionalWork = null)
        {
            if (additionalWork is null)
            {
                // Nothing else to coordinate, so a single save is the whole operation.
                var affected = await SaveAttachmentChangesAsync(unitOfWork, attachmentService, resolution);

                if (affected > 0)
                    RetireReplacedFile(attachmentService, resolution);

                return affected;
            }

            await using var transaction = await unitOfWork.BeginTransactionAsync();

            var rowsAffected = 0;

            try
            {
                rowsAffected = await SaveAttachmentChangesAsync(unitOfWork, attachmentService, resolution);

                if (rowsAffected == 0)
                {
                    await RollbackQuietlyAsync(transaction);
                    return 0;
                }

                await additionalWork();

                await transaction.CommitAsync();
            }
            catch
            {
                // Nothing this request wrote survives, so the upload has to go back too and the
                // file the row still points at is left exactly where it was.
                await RollbackQuietlyAsync(transaction);
                DiscardUploadedFile(attachmentService, resolution);
                throw;
            }

            // Committed, so the row no longer references the file it replaced.
            RetireReplacedFile(attachmentService, resolution);

            return rowsAffected;
        }

        /// <summary>
        /// Performs the attachment write itself, undoing the upload if it does not land. It never
        /// touches the file being replaced; that is <see cref="CommitAttachmentChangeAsync"/>'s job
        /// and only happens once the work is known to be durable.
        /// </summary>
        private static async Task<int> SaveAttachmentChangesAsync(
            IUnitOfWork unitOfWork,
            IAttachmentService attachmentService,
            AttachmentResolution resolution)
        {
            try
            {
                var affected = await unitOfWork.SaveChangesAsync();

                if (affected == 0)
                {
                    // The row still points at the file it had, so the upload has to go back.
                    DiscardUploadedFile(attachmentService, resolution);
                }

                return affected;
            }
            catch
            {
                DiscardUploadedFile(attachmentService, resolution);
                throw;
            }
        }

        private static async Task RollbackQuietlyAsync(IUnitOfWorkTransaction transaction)
        {
            try
            {
                await transaction.RollbackAsync();
            }
            catch
            {
                // Already unwinding a failure; the original exception is the one worth reporting.
            }
        }

        private static void RetireReplacedFile(IAttachmentService attachmentService, AttachmentResolution resolution)
        {
            if (!string.IsNullOrWhiteSpace(resolution.ReplacedPath))
                attachmentService.Delete(resolution.ReplacedPath);
        }

        /// <summary>
        /// Removes the row and the file it owned. The file is only unlinked once the row is
        /// actually gone, so a failed delete cannot leave the database pointing at nothing.
        /// </summary>
        public static async Task<bool> DeleteWithAttachmentsAsync<TEntity, TKey>(
            this IGenericRepository<TEntity, TKey> repository,
            IUnitOfWork unitOfWork,
            IAttachmentService attachmentService,
            TKey id,
            Func<TEntity, string?> storedPathSelector)
            where TEntity : BaseEntity<TKey>
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null) return false;

            var storedPath = storedPathSelector(entity);

            repository.Delete(entity);

            var deleted = await unitOfWork.SaveChangesAsync() > 0;

            if (deleted && !string.IsNullOrWhiteSpace(storedPath))
                attachmentService.Delete(storedPath);

            return deleted;
        }

        /// <summary>
        /// The only form an attachment path is ever persisted in. A stored relative path is kept
        /// as-is; a host or the request prefix is stripped back out, so a full URL can never reach
        /// the database even if one is passed in.
        /// </summary>
        public static string ToStoredPath(this AttachmentOptions attachmentOptions, string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            var candidate = url.Replace('\\', '/');

            // An absolute URL carries a host and a mount point that have no business in a column.
            var relativePath = Uri.TryCreate(candidate, UriKind.Absolute, out var uri)
                ? uri.AbsolutePath
                : candidate;

            // Compare with the mount point reduced to a bare prefix so "/Files/x" and "Files/x" match.
            var requestPath = (attachmentOptions.RequestPath ?? string.Empty).Trim().Trim('/');
            relativePath = relativePath.TrimStart('/');

            if (requestPath.Length == 0)
                return relativePath;

            // Require a separator boundary so a folder that merely starts with the same letters is kept.
            if (relativePath.StartsWith(requestPath + "/", StringComparison.OrdinalIgnoreCase))
                return relativePath[(requestPath.Length + 1)..];

            return relativePath.Equals(requestPath, StringComparison.OrdinalIgnoreCase)
                ? string.Empty
                : relativePath;
        }

        private static void DiscardUploadedFile(IAttachmentService attachmentService, AttachmentResolution resolution)
        {
            if (resolution.StoredNewFile && !string.IsNullOrWhiteSpace(resolution.StoredPath))
                attachmentService.Delete(resolution.StoredPath);
        }
    }
}