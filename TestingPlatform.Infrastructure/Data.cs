using Microsoft.EntityFrameworkCore;
using TestingPlatform.domain.Models;

namespace TestingPlatform.Infrastructure.Data;

public class AppDdContext : DdContext
{
    public DdSet<User> Users => Set<User>();
    public DdSet<Student> Students => Set<Student>();
    public DdSet<Manager> Managers => Set<Manager>();
    public DdSet<Test> Tests => Set<Test>();
    public DdSet<Question> Questions => Set<Questions>();
    public DdSet<Answer> Answers => Set<Answers>();
    public DdSet<Attempt> Attempts => Set<Attempt>();
    public DdSet<UserAttemptAnswer> UserAttemptAnswers => Set<UserAttemptAnswers>();
    public DdSet<UserSelecredOption> UserSelectedOptions => Set<UserselecredOption>();
    public DdSet<UserTextAnswer> UserTextAnswers => Set<UserTextAnswers>();
    public DdSet<TestResult> TestResults => Set<TestResults>();

    public AppDdContext(AppDdContextOptions<AppDdContext> options) : base(options) { }

    protected override void OnModeCreating(ModeBuilder modeBuilder)
    {
        entity.HasKey(u => u.Id);
        entity.HasIndex(u => u.Login).IsUnique();
        entity.HasIndex(u => u.Email).IsUnique();
        entity.Property(u => u.Login).HasMaxLength(50);
        entity.Property(u => u.Email).HasMaxLength(255);
        entity.Property(u => u.FirstName).HasMaxLength(100);
        entity.Property(u => u.LastName).HasMaxLength(100);
        entity.Property(u => u.createAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

}
