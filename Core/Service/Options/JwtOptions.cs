namespace Service.Options
{
    /// <summary>
    /// How admin access tokens are signed, bound from the <c>Jwt</c> configuration section.
    /// Issuer, audience and lifetime are all configuration so they can be changed without touching
    /// code, and the key itself is read from the same place rather than hard-coded in a service.
    /// </summary>
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; set; } = "Portofolio";

        public string Audience { get; set; } = "PortofolioClient";

        /// <summary>
        /// Symmetric signing secret. HMAC-SHA256 keys must be at least 256 bits, so anything
        /// shorter is rejected at startup instead of silently producing weak tokens.
        /// </summary>
        public string SigningKey { get; set; } = string.Empty;

        /// <summary>How long a freshly issued token stays valid.</summary>
        public int DurationInMinutes { get; set; } = 120;

        public DateTime ExpiresAtUtc => DateTime.UtcNow.AddMinutes(DurationInMinutes);
    }
}
