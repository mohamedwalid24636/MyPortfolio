using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

/// <summary>
/// The single account allowed to reach the admin API.
///
/// This is a plain <see cref="IdentityUser"/>, so the password is never stored: Identity keeps only
/// a salted PBKDF2 hash produced by <c>UserManager</c>, and every login re-hashes the submitted
/// password against it. There are no profile fields of our own because nothing else in the system
/// needs to know who is signed in — possession of a valid token is the whole authorisation story.
/// </summary>
public class ApplicationUser : IdentityUser
{
}
