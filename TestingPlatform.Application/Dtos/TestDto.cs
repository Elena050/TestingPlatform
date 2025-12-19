namespace TestingPlatform.Application.Dtos;

public class TestDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsRepeatable { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTimeOffset PublishedAt { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public int? DurationMinutes { get; set; }
    public int? PassingScore { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

