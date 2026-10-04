using Microsoft.AspNetCore.Http;

namespace ServiceAbstraction
{
    /// <summary>
    /// The single seam for every file the solution stores. Nothing else in the solution is allowed
    /// to touch the disk, so folder selection, naming, validation and cleanup all stay in one place.
    /// </summary>
    public interface IAttachmentService : IAttachementService
    {
    }

    /// <summary>The upload and delete contract, kept under its original name.</summary>
    public interface IAttachementService
    {
        /// <summary>
        /// Stores the file under the server-chosen folder and returns the relative path to persist,
        /// or <c>null</c> when the file is not acceptable.
        /// </summary>
        Task<string?> Upload(IFormFile File, string FolderName);

        /// <summary>Removes a stored file. Returns false when there was nothing to remove.</summary>
        bool Delete(string FilePath);
    }
}
