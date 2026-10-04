using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class AuthResponseDto
{
    /// <summary>Bearer token the client sends as <c>Authorization: Bearer &lt;token&gt;</c>.</summary>
    public string Token { get; set; }

    /// <summary>When the token stops being accepted. The client uses this to skip a dead session.</summary>
    public DateTime ExpiresAtUtc { get; set; }

    public string Email { get; set; }
}
