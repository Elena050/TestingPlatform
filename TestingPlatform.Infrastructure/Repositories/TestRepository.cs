using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Domain.Models;
using TestingPlatform.Infrastructure.Data;

namespace TestingPlatform.Infrastructure.Repositories;

public class TestRepository : ITestRepository
{
    private readonly AppDbContext _context;

    public TestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Test>> GetAllAsync()
    {
        return await _context.Tests
            .Include(t => t.CreatedByManager)
                .ThenInclude(m => m.User)
            .Include(t => t.Questions)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Test?> GetByIdAsync(int id)
    {
        return await _context.Tests
            .Include(t => t.CreatedByManager)
                .ThenInclude(m => m.User)
            .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<int> CreateAsync(Test test)
    {
        _context.Tests.Add(test);
        await _context.SaveChangesAsync();
        return test.Id;
    }

    public async Task UpdateAsync(Test test)
    {
        _context.Tests.Update(test);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var test = await _context.Tests.FindAsync(id);
        if (test != null)
        {
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Test>> GetPublishedTestsAsync()
    {
        return await _context.Tests
            .Where(t => t.PublishedAt <= DateTimeOffset.UtcNow && t.IsPublic)
            .Include(t => t.CreatedByManager)
                .ThenInclude(m => m.User)
            .OrderByDescending(t => t.PublishedAt)
            .ToListAsync();
    }

    public async Task<List<Test>> GetTestsByManagerAsync(int managerId)
    {
        return await _context.Tests
            .Where(t => t.CreatedByManagerId == managerId)
            .Include(t => t.Questions)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}

