namespace Service.Options
{
    /// <summary>
    /// The one admin account created on startup, bound from the <c>AdminSeed</c> configuration
    /// section. It is only ever used when no account with that email exists yet, so restarting the
    /// API never resets a password that has since been changed.
    /// </summary>
    public class AdminSeedOptions
    {
        public const string SectionName = "AdminSeed";

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        /// <summary>Seeding is skipped entirely when no credentials are configured.</summary>
        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
    }
}
