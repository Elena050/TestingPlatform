namespace TestingPlatform.Application.Dtos;

public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Number { get; set; }
    public string AnswerType { get; set; } = string.Empty;
    public List<AnswerDto> Answers { get; set; } = new();
}
