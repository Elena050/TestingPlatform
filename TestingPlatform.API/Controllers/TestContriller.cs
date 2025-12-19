using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Domain.Enums;
using TestingPlatform.Domain.Models;

namespace TestingPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TestsController : ControllerBase
{
    private readonly ITestRepository _testRepository;
    private readonly ILogger<TestsController> _logger;

    public TestsController(ITestRepository testRepository, ILogger<TestsController> logger)
    {
        _testRepository = testRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<TestDto>>> GetAllTests()
    {
        try
        {
            var tests = await _testRepository.GetPublishedTestsAsync();
            
            var testDtos = tests.Select(test => new TestDto
            {
                Id = test.Id,
                Title = test.Title,
                Description = test.Description,
                IsRepeatable = test.IsRepeatable,
                Type = test.Type.ToString(),
                PublishedAt = test.PublishedAt,
                Deadline = test.Deadline,
                DurationMinutes = test.DurationMinutes,
                PassingScore = test.PassingScore,
                CreatedBy = $"{test.CreatedByManager.User.FirstName} {test.CreatedByManager.User.LastName}"
            }).ToList();

            return Ok(testDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении списка тестов");
            return StatusCode(500, "Произошла внутренняя ошибка сервера");
        }
    }

комп, [18.12.2025 16:23]
[HttpGet("{id:int}")]
    public async Task<ActionResult<TestDto>> GetTestById(int id)
    {
        var test = await _testRepository.GetByIdAsync(id);
        if (test == null)
        {
            return NotFound($"Тест с ID {id} не найден");
        }

        var testDto = new TestDto
        {
            Id = test.Id,
            Title = test.Title,
            Description = test.Description,
            IsRepeatable = test.IsRepeatable,
            Type = test.Type.ToString(),
            PublishedAt = test.PublishedAt,
            Deadline = test.Deadline,
            DurationMinutes = test.DurationMinutes,
            PassingScore = test.PassingScore,
            CreatedBy = $"{test.CreatedByManager.User.FirstName} {test.CreatedByManager.User.LastName}"
        };

        return Ok(testDto);
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<TestDto>> CreateTest([FromBody] CreateTestRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var managerId = 1;

            if (!Enum.TryParse<TestType>(request.Type, out var testType))
            {
                return BadRequest("Неверный тип теста");
            }

            var test = new Test
            {
                Title = request.Title,
                Description = request.Description,
                Type = testType,
                IsRepeatable = request.IsRepeatable,
                DurationMinutes = request.DurationMinutes,
                PassingScore = request.PassingScore,
                MaxAttempts = request.MaxAttempts,
                PublishedAt = request.PublishedAt,
                Deadline = request.Deadline,
                IsPublic = true,
                CreatedByManagerId = managerId,
                Questions = request.Questions.Select(q => new Question
                {
                    Text = q.Text,
                    Number = q.Number,
                    AnswerType = Enum.Parse<AnswerType>(q.AnswerType),
                    Answers = q.Answers.Select(a => new Answer
                    {
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList(),
                CreatedAt = DateTimeOffset.UtcNow
            };

            var testId = await _testRepository.CreateAsync(test);
            
            return CreatedAtAction(nameof(GetTestById), new { id = testId }, new { Id = testId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании теста");
            return StatusCode(500, "Произошла ошибка при создании теста");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateTest(int id, [FromBody] CreateTestRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existingTest = await _testRepository.GetByIdAsync(id);
            if (existingTest == null)
            {
                return NotFound();
            }

            existingTest.Title = request.Title;
            existingTest.Description = request.Description;
            
            await _testRepository.UpdateAsync(existingTest);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении теста");
            return StatusCode(500, "Произошла ошибка при обновлении теста");
        }
    }
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteTest(int id)
    {
        try
        {
            await _testRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении теста");
            return StatusCode(500, "Произошла ошибка при удалении теста");
        }
    }
}
