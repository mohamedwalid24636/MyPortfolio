using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Options;

namespace Service.Attachments
{
    /// <summary>
    /// Turns the relative path held in the database into the URL the client fetches. It reads
    /// configuration only and never touches the disk, which is what keeps it separate from
    /// <see cref="Services.AttachementService"/>: the write path produces paths, the read path
    /// produces URLs.
    /// </summary>
    public class AttachmentUrls(IConfiguration _configuration, IOptions<AttachmentOptions> _options)
    {
        private readonly string _baseUrl = (_configuration["Urls:BaseUrl"] ?? string.Empty).TrimEnd('/');
        private readonly string _requestPath = (_options.Value.RequestPath ?? string.Empty).TrimEnd('/');

        /// <summary>
        /// Composes the absolute URL for a stored path. A path that is already absolute is passed
        /// through untouched so a client never gets a URL prefixed twice.
        /// </summary>
        public string Resolve(string? storedPath)
        {
            if (string.IsNullOrWhiteSpace(storedPath))
                return string.Empty;

            var relativePath = storedPath.Replace('\\', '/').TrimStart('/');

            if (relativePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || relativePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return relativePath;

            return $"{_baseUrl}{_requestPath}/{relativePath}";
        }
    }
}
