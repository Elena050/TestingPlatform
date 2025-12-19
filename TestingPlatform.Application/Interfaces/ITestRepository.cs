using TestingPlatform.Domain.Models;

namespace TestingPlatform.Application.Interfaces;

public interface ITestRepository
{
    Task<List<Test>> GetAllAsync();
    Task<Test?> GetByIdAsync(int id);
    Task<int> CreateAsync(Test test);
    Task UpdateAsync(Test test);
    Task DeleteAsync(int id);
    Task<List<Test>> GetPublishedTestsAsync();
    Task<List<Test>> GetTestsByManagerAsync(int managerId);
}
