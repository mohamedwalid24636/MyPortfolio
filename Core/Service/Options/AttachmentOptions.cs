namespace Service.Options
{
    /// <summary>
    /// Server-side rules for every uploaded file, bound from the <c>Attachments</c> configuration
    /// section. Which types are accepted, how large they may be, where they land and how they are
    /// addressed are all decided here, so none of it has to be trusted from or sent by the client.
    /// </summary>
    public class AttachmentOptions
    {
        public const string SectionName = "Attachments";

        /// <summary>Folder under the web root that holds every attachment.</summary>
        public string RootPath { get; set; } = "Files";

        /// <summary>Path the attachment folder is served from, so stored paths resolve to a URL.</summary>
        public string RequestPath { get; set; } = "/Files";

        /// <summary>Size ceiling applied to any extension without a specific entry below.</summary>
        public long MaxSizeInBytes { get; set; } = 10 * 1024 * 1024;

        /// <summary>Per-extension overrides of <see cref="MaxSizeInBytes"/>.</summary>
        public Dictionary<string, long> MaxSizePerExtensionInBytes { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Extensions the server will store at all.</summary>
        public List<string> AllowedExtensions { get; set; } =
        [
            ".jpg", ".jpeg", ".png", ".webp", ".gif", ".svg", ".pdf", ".mp4"
        ];

        /// <summary>Maps an <see cref="Attachments.AttachmentFolderKeys"/> value to its relative folder.</summary>
        public Dictionary<string, string> Folders { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Resolves a logical folder key, falling back to the key itself.</summary>
        public string GetFolder(string folderKey)
            => Folders.TryGetValue(folderKey, out var folder) && !string.IsNullOrWhiteSpace(folder)
                ? folder
                : folderKey;

        public bool IsAllowedExtension(string extension)
            => AllowedExtensions.Any(allowed => allowed.Equals(extension, StringComparison.OrdinalIgnoreCase));

        /// <summary>Ceiling for an extension, honouring a per-extension override when present.</summary>
        public long GetMaxSizeInBytes(string extension)
            => MaxSizePerExtensionInBytes.TryGetValue(extension, out var size) ? size : MaxSizeInBytes;
    }
}
