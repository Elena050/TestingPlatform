using Microsoft.EntityFrameworkCore;
using TestingPlatform.Domain.Models;

namespace TestingPlatform.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Manager> Managers => Set<Manager>();
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<Attempt> Attempts => Set<Attempt>();
    public DbSet<UserAttemptAnswer> UserAttemptAnswers => Set<UserAttemptAnswer>();
    public DbSet<UserSelectedOption> UserSelectedOptions => Set<UserSelectedOption>();
    public DbSet<UserTextAnswer> UserTextAnswers => Set<UserTextAnswer>();
    public DbSet<TestResult> TestResults => Set<TestResult>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Login).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Login).HasMaxLength(50);
            entity.Property(u => u.Email).HasMaxLength(255);
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasIndex(s => s.UserId).IsUnique();
            entity.HasOne(s => s.User)
                  .WithOne(u => u.Student)
                  .HasForeignKey<Student>(s => s.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(s => s.Phone).HasMaxLength(30);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.HasIndex(m => m.UserId).IsUnique();
            entity.HasOne(m => m.User)
                  .WithOne(u => u.Manager)
                  .HasForeignKey<Manager>(m => m.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.HasOne(t => t.CreatedByManager)
                  .WithMany(m => m.CreatedTests)
                  .HasForeignKey(t => t.CreatedByManagerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(q => q.Id);
            entity.HasIndex(q => new { q.TestId, q.Number }).IsUnique();
            entity.HasOne(q => q.Test)
                  .WithMany(t => t.Questions)
                  .HasForeignKey(q => q.TestId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.HasOne(a => a.Question)
                  .WithMany(q => q.Answers)
                  .HasForeignKey(a => a.QuestionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Attempt>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.HasIndex(a => new { a.StudentId, a.TestId });
            entity.HasOne(a => a.Student)
                  .WithMany(s => s.Attempts)
                  .HasForeignKey(a => a.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(a => a.Test)
                  .WithMany(t => t.Attempts)
                  .HasForeignKey(a => a.TestId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserAttemptAnswer>(entity =>
        {
            entity.HasKey(ua => ua.Id);
            entity.HasIndex(ua => new { ua.AttemptId, ua.QuestionId }).IsUnique();
            entity.HasOne(ua => ua.Attempt)
                  .WithMany(a => a.UserAttemptAnswers)
                  .HasForeignKey(ua => ua.AttemptId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(ua => ua.Question)
                  .WithMany(q => q.UserAttemptAnswers)
                  .HasForeignKey(ua => ua.QuestionId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(tr => tr.Id);
            entity.HasIndex(tr => new { tr.TestId, tr.StudentId, tr.AttemptId }).IsUnique();
        });

        // Seed data (как в файле стр. 35)
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = 1, 
                Login = "manager", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"),
                Email = "manager@school.ru", 
                FirstName = "Анна", 
                LastName = "Иванова", 
                Role = Domain.Enums.UserRole.Manager,
                CreatedAt = DateTimeOffset.UtcNow
            },
            new User 
            { 
                Id = 2, 
                Login = "student", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
                Email = "student@school.ru", 
                FirstName = "Петр", 
                LastName = "Сидоров", 
                Role = Domain.Enums.UserRole.Student,
                CreatedAt = DateTimeOffset.UtcNow
            }
        );

        modelBuilder.Entity<Manager>().HasData(
            new Manager { Id = 1, UserId = 1 }
        );

        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, UserId = 2, Phone = "+71234567890", VkProfileLink = "http://vk.com/student1" }
        );
    }
}
