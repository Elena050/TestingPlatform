using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Application.Dtos;

public class CreateQuestionRequest
{
    [Required]
    public string Text { get; set; } = string.Empty;
    
    [Required]
    public string AnswerType { get; set; } = "Single";
    
    [Range(1, 100)]
    public int Number { get; set; }
    
    public List<CreateAnswerRequest> Answers { get; set; } = new();
}
