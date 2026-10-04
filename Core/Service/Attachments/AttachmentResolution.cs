namespace Service.Attachments
{
    /// <summary>
    /// The outcome of resolving one attachment change, carried from the point the file is written
    /// to the point the row is saved, so the two can be kept in step.
    /// </summary>
    public class AttachmentResolution
    {
        /// <summary>
        /// Relative path to persist on the entity, or empty when the attachment was removed.
        /// Already stripped of any host and request prefix, so it is safe to assign directly.
        /// </summary>
        public string StoredPath { get; init; } = string.Empty;

        /// <summary>True when a new file reached the disk during this change.</summary>
        public bool StoredNewFile { get; init; }

        /// <summary>
        /// The file the entity used to point at, when this change moves it somewhere else: either
        /// onto a newly uploaded file or onto nothing at all because the attachment was deleted.
        /// Nothing is removed while resolving; this path is retired only after the row that stops
        /// referencing it has been written successfully.
        /// </summary>
        public string? ReplacedPath { get; init; }

        /// <summary>Metadata of the stored file, taken from the upload rather than the request.</summary>
        public string? FileName { get; init; }

        public string? FileType { get; init; }
    }
}
