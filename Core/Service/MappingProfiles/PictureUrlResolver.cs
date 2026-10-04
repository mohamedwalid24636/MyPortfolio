using AutoMapper;
using Service.Attachments;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Service.MappingProfiles
{
    /// <summary>
    /// Reference implementation for exposing a stored attachment to the client. It reads the
    /// relative path the entity holds and hands it to <see cref="AttachmentUrls"/> to become a
    /// complete URL.
    /// <para>
    /// This is the mapping layer's job by design: the entity and the database only ever hold a
    /// relative path, and no service builds a URL by hand.
    /// </para>
    /// </summary>
    public class PictureUrlResolver<TSource, TDestination>(AttachmentUrls attachmentUrls, string storedPathMember)
        : IValueResolver<TSource, TDestination, string>
    {
        private static readonly ConcurrentDictionary<string, Func<TSource, string?>> StoredPathReaders = new();

        private readonly Func<TSource, string?> _readStoredPath =
            StoredPathReaders.GetOrAdd(storedPathMember, BuildStoredPathReader);

        public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
            => attachmentUrls.Resolve(_readStoredPath(source));

        private static Func<TSource, string?> BuildStoredPathReader(string storedPathMember)
        {
            var property = typeof(TSource).GetProperty(storedPathMember, BindingFlags.Public | BindingFlags.Instance)
                ?? throw new InvalidOperationException(
                    $"'{typeof(TSource).Name}' has no attachment member named '{storedPathMember}'.");

            if (property.PropertyType != typeof(string))
                throw new InvalidOperationException(
                    $"'{typeof(TSource).Name}.{storedPathMember}' must be a string to be resolved as an attachment.");

            var parameter = Expression.Parameter(typeof(TSource), "source");
            var body = Expression.Convert(Expression.Property(parameter, property), typeof(string));

            return Expression.Lambda<Func<TSource, string?>>(body, parameter).Compile();
        }
    }
}
