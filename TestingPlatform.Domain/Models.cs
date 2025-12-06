using TestingPlatform.Domain.Enums;

namespace TestingPlatform.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName{ get; set; }
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTimeOffset CreateAt { get; set; } = DatetimeOffset.UtcNow;
    public Student? Student { get; set; }
    public Manager? Manager { get; set; }
}

public class Student
{
    public int Id { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string VkProfileLink { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public List<Attempt> Attempts { get; set; } = new();
    public List<TestResult> TestResults { get; set; } = new();
}

public class Manager
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public List<Test> CreatedTests { get; set; } = new();
}
public class Test
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsRepeatable { get; set; }
    public TestType Type { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset PublishedAt { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public int? DurationMinutes { get; set; }
    public bool IsPublic { get; set; } = false;
    public int? PassingScore { get; set; }
    public int? MaxAttempts { get; set; }
    public int CreatedByManagerId { get; set; }
    public Manager CreatedByManager { get; set; } = null!;
    public List<Question> Questions { get; set; } = new();
    public List<Attempt> Attempts { get; set; } = new();
    public List<TestResult> TestResults { get; set; } = new();
}

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Number { get; set; }
    public string? Description { get; set; }
    public AnswerType AnswerTyoe { get; set; }
    public bool IsScoring { get; set; } = true;
    public int? MaxScore { get; set; }
    public int  TestId { get; set; }
    public Test Test { get; set; } = null!;
    public List<UserAttemptAnswer> UserAttemptAnswers { get; set; } = new();
    public List<Answer> Answers { get; set; } = new();
}

public class Answer
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public List<UserSelectedOption> UserSelectedOptions { get; set; } = new();
}

public class Attempt
{
    public int Id { get; set; }
    public DateTimeOffset StartedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? SubmittedAt { get; set; }
    public int? Score { get; set; }
    public int TestId { get; set; }
    public int StudentId { get; set; }
    public Test Test { get; set; } = null!;
    public Student Student { get; set; } = null!;
    public List<UserAttemptAnswer> UserAttemptAnswers { get; set; } = new();
}

public class UserAttemptAnswer
{
    public int Id { get; set; }
    public bool? IsCorrect { get; set; }
    public int? ScoreAwarded { get; set; }
    public int AttemptId { get; set; }
    public int QuestionId { get; set; }
    public Attempt Attempt { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public list<UserSelectedOption> UserSelectedOptions { get; set; } = new();
    public UserTextAnswer? UserTextAnswer { get; set; }
}

public class UserSelectedOption
{
    public int Id { get; set; }
    public int UserAttemptAnswerId { get; set; }
    public int AnswerId { get; set; }
    public Answer Answer { get; set; } = null!;
}

public class UserTextAnswer
{
    public int Id { get; set; }
    public string TextAnswer { get; set; } = string.Empty;
    public int UserAttemptAnswerId { get; set; }
    public UserAttemptAnswer UserAttemptAnswer { get; set; } = null!;
}

public class TestResult
{
    public int Id { get; set; }
    public bool Passed { get; set; }
    public int TestId { get; set; }
    public int AttemptId { get; set; }
    public int StudentId { get; set; }
    public Test Test { get; set; } = null!;
    public Attempt Attempt { get; set; } = null!;
    public Student Student { get; set; } = null!;
}
