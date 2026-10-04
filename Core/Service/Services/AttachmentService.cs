using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Options;
using ServiceAbstraction;
using ServiceAbstraction.Attachments;

namespace Service.Services
{
    /// <summary>
    /// The only component in the solution that reads or writes files. It owns the storage rules:
    /// which extensions are accepted, how large a file may be, which folder it lands in and what
    /// it gets named. Everything it hands back is a relative path, never a client-facing URL.
    /// </summary>
    public class AttachementService(IConfiguration _configuration, IOptions<AttachmentOptions> _options) : IAttachmentService
    {
        private readonly AttachmentOptions _attachmentOptions = _options.Value;

        public bool Delete(string FileName)
        {
            if (string.IsNullOrWhiteSpace(FileName))
                return false;

            var relativePath = ToRelativePath(FileName);

            // Uploads live under the configured root; the bare web root is kept as a fallback so
            // files stored before the root was configured can still be cleaned up.
            foreach (var root in new[] { _attachmentOptions.RootPath, string.Empty })
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", root, relativePath);

                if (!File.Exists(filePath))
                    continue;

                File.Delete(filePath);
                return true;
            }

            return false;
        }

        public async Task<string?> Upload(IFormFile File, string FolderName)
        {
            var extension = Path.GetExtension(File.FileName).ToLowerInvariant();

            if (!_attachmentOptions.IsAllowedExtension(extension))
                throw new AttachmentException($"'{extension}' files are not an accepted attachment type.");

            var maxSize = _attachmentOptions.GetMaxSizeInBytes(extension);

            if (File.Length == 0)
                throw new AttachmentException("The uploaded file is empty.");

            if (File.Length > maxSize)
                throw new AttachmentException(
                    $"'{extension}' attachments are limited to {maxSize / 1024 / 1024} MB.");

            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot", _attachmentOptions.RootPath, FolderName);

            Directory.CreateDirectory(folderPath);

            // The client supplies the original name only; the stored name is ours, so a client can
            // never steer the file onto a path of its choosing.
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(File.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);

            await File.CopyToAsync(stream);

            return $"{FolderName}/{fileName}";
        }

        /// <summary>
        /// Reduces whatever it is handed to the relative path that was stored, so an absolute URL
        /// reaching a delete call cannot escape the attachment folder.
        /// </summary>
        private string ToRelativePath(string fileName)
        {
            var relativePath = fileName.Replace('\\', '/').TrimStart('/');

            foreach (var prefix in new[] { _configuration["Urls:BaseUrl"], _attachmentOptions.RequestPath })
            {
                if (string.IsNullOrWhiteSpace(prefix))
                    continue;

                var normalized = prefix.Replace('\\', '/').TrimEnd('/');

                if (relativePath.StartsWith(normalized, StringComparison.OrdinalIgnoreCase))
                    relativePath = relativePath[normalized.Length..].TrimStart('/');
            }

            return relativePath;
        }
    }
}
