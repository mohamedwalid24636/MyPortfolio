using AutoMapper;
using Service.Attachments;
using System.Linq.Expressions;

namespace Service.MappingProfiles
{
    /// <summary>
    /// Gives a mapping profile the attachment URL resolver for its entity and read DTO pair, so
    /// every stored relative path is exposed through the same
    /// <see cref="PictureUrlResolver{TSource, TDestination}"/> instead of each profile building
    /// URLs on its own.
    /// </summary>
    public abstract class AttachmentProfile<TSource, TDestination> : AutoMapper.Profile
    {
        private readonly AttachmentUrls _attachmentUrls;

        protected AttachmentProfile(AttachmentUrls attachmentUrls)
            => _attachmentUrls = attachmentUrls;

        /// <summary>
        /// Resolves the stored relative path behind a destination member to the URL the client
        /// fetches. The member is taken from the expression rather than a string, so it is checked
        /// by the compiler and follows a rename.
        /// </summary>
        protected IValueResolver<TSource, TDestination, string> AttachmentUrlFor<TMember>(
            Expression<Func<TDestination, TMember>> destinationMember)
            => new PictureUrlResolver<TSource, TDestination>(_attachmentUrls, MemberName(destinationMember));

        private static string MemberName<TMember>(Expression<Func<TDestination, TMember>> destinationMember)
            => destinationMember.Body is MemberExpression member
                ? member.Member.Name
                : throw new InvalidOperationException(
                    $"'{destinationMember}' does not refer to a member of '{typeof(TDestination).Name}'.");
    }
}
