using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Options;
using ServiceAbstraction;
using Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services
{
    /// <summary>
    /// Issues admin access tokens.
    ///
    /// Verification is delegated to <see cref="UserManager{TUser}"/>, which is what makes Identity
    /// worth using here: the stored password is a salted PBKDF2 hash and
    /// <c>CheckPasswordSignInAsync</c> compares against it in constant time. No plaintext or
    /// comparison of our own appears anywhere in this class.
    /// </summary>
    public class AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtOptions> jwtOptions)
        : IAuthService
    {
        private readonly JwtOptions _jwt = jwtOptions.Value;

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email.Trim());

            // Lockout is deliberately not requested. There is exactly one admin account and no
            // password-reset flow to recover it, so five typos would otherwise lock the owner out
            // of their own panel with no way back in short of editing the database. Rate limiting
            // the login route is the right place to slow guessing down instead.
            if (user is null) return null;

            // CheckPasswordAsync re-hashes the submitted password with the user's salt and compares
            // in constant time, so neither the stored hash nor the password leaks through timing. A
            // wrong password and an unknown email both arrive here as the same null.
            var passwordCheck = await userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordCheck) return null;

            var expiresAtUtc = _jwt.ExpiresAtUtc;

            return new AuthResponseDto
            {
                Token = CreateToken(user, expiresAtUtc),
                ExpiresAtUtc = expiresAtUtc,
                Email = user.Email ?? string.Empty,
            };
        }

        private string CreateToken(ApplicationUser user, DateTime expiresAtUtc)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                // A unique id per token, so two logins are distinguishable when auditing.
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAtUtc,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
