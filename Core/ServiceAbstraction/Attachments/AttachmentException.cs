namespace ServiceAbstraction.Attachments
{
    /// <summary>
    /// Raised when an attachment cannot be accepted: a rejected extension, a file over the limit,
    /// or a storage failure. The client did something the server will not store, so this is a bad
    /// request rather than a server fault.
    /// </summary>
    public class AttachmentException : Exception
    {
        public AttachmentException(string message) : base(message)
        {
        }

        public AttachmentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
