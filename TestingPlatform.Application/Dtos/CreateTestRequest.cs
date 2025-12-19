using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Application.Dtos;

public class CreateTestRequest
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public bool IsRepeatable { get; set; } = false;
    public string Type { get; set; } = "Education";
    public int? DurationMinutes { get; set; }
    public int? PassingScore { get; set; }
    public int? MaxAttempts { get; set; }
    
    [Required]
    public DateTimeOffset PublishedAt { get; set; }
    
    public DateTimeOffset? Deadline { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreateQuestionRequest> Questions { get; set; } = new();
}
