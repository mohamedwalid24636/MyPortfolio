using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Attachments;

namespace Presentation.Handlers
{
    /// <summary>
    /// Reports a rejected attachment as a bad request instead of letting it surface as a server
    /// fault. Only <see cref="AttachmentException"/> is handled here; anything else keeps its
    /// existing treatment.
    /// </summary>
    public class AttachmentExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not AttachmentException attachmentException)
                return false;

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Attachment rejected",
                    Detail = attachmentException.Message
                }
            });
        }
    }
}
