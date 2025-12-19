using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Application.Dtos;

public class LoginRequest
{
    [Required]
    public string Login { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}
