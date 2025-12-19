using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Application.Dtos;

public class CreateAnswerRequest
{
    [Required]
    public string Text { get; set; } = string.Empty;
    
    public bool IsCorrect { get; set; }
}
