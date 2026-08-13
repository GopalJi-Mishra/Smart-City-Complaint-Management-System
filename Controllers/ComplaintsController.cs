using Microsoft.AspNetCore.Mvc;
using SmartCityComplaint.Data;
using SmartCityComplaint.Models;
using SmartCityComplaint.Services;

namespace SmartCityComplaint.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComplaintsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly GeminiService _geminiService;

    public ComplaintsController(
        ApplicationDbContext context,
        GeminiService geminiService)
    {
        _context = context;
        _geminiService = geminiService;
    }


    // CREATE COMPLAINT
    [HttpPost]
    public async Task<IActionResult> CreateComplaint(
        [FromForm] string description,
        [FromForm] string location,
        [FromForm] int userId,
        [FromForm] IFormFile? image)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return BadRequest("Complaint description is required.");
        }

        if (string.IsNullOrWhiteSpace(location))
        {
            return BadRequest("Location is required.");
        }


        // Gemini analyzes complaint text and optional image
        string aiResult =
            await _geminiService.AnalyzeComplaint(
                description,
                image
            );


        // Extract category and priority from AI result
        string category = ExtractCategory(aiResult);

        string priority = ExtractPriority(aiResult);


        var complaint = new Complaint
        {
            Description = description,
            Location = location,
            Category = category,
            Priority = priority,
            Status = "Pending",
            Department = "",
            UserId = userId,
            AIAnalysis = aiResult
        };


        _context.Complaints.Add(complaint);

        await _context.SaveChangesAsync();


        return Ok(complaint);
    }


    // GET COMPLAINTS
    [HttpGet]
    public IActionResult GetComplaints([FromQuery] int? userId)
    {
        if (userId.HasValue)
        {
            var userComplaints = _context.Complaints
                .Where(c => c.UserId == userId.Value)
                .ToList();

            return Ok(userComplaints);
        }


        var complaints =
            _context.Complaints.ToList();

        return Ok(complaints);
    }


    // UPDATE STATUS
    [HttpPut("{id}/status")]
    public IActionResult UpdateStatus(
        int id,
        [FromQuery] string status)
    {
        var complaint =
            _context.Complaints.Find(id);


        if (complaint == null)
        {
            return NotFound("Complaint not found");
        }


        complaint.Status = status;

        _context.SaveChanges();


        return Ok(complaint);
    }


    // ASSIGN DEPARTMENT
    [HttpPut("{id}/department")]
    public IActionResult AssignDepartment(
        int id,
        [FromQuery] string department)
    {
        var complaint =
            _context.Complaints.Find(id);


        if (complaint == null)
        {
            return NotFound("Complaint not found");
        }


        complaint.Department = department;

        _context.SaveChanges();


        return Ok(complaint);
    }


    // TEST AI
    [HttpGet("test-ai")]
    public async Task<IActionResult> TestAI()
    {
        string result =
            await _geminiService.AnalyzeComplaint(
                "There is a broken traffic signal at a busy intersection."
            );

        return Ok(result);
    }


    // EXTRACT CATEGORY
    private string ExtractCategory(string aiResult)
    {
        foreach (string line in aiResult.Split('\n'))
        {
            string trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("Category:", StringComparison.OrdinalIgnoreCase))
            {
                string category =
                    trimmedLine.Substring("Category:".Length).Trim();

                if (!string.IsNullOrWhiteSpace(category))
                {
                    return category;
                }
            }
        }

        return "Other";
    }


    // EXTRACT PRIORITY
    private string ExtractPriority(string aiResult)
    {
        foreach (string line in aiResult.Split('\n'))
        {
            string trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("Priority:", StringComparison.OrdinalIgnoreCase))
            {
                string priority =
                    trimmedLine.Substring("Priority:".Length).Trim();

                if (priority.Equals("High", StringComparison.OrdinalIgnoreCase))
                {
                    return "High";
                }

                if (priority.Equals("Medium", StringComparison.OrdinalIgnoreCase))
                {
                    return "Medium";
                }

                if (priority.Equals("Low", StringComparison.OrdinalIgnoreCase))
                {
                    return "Low";
                }
            }
        }

        return "Low";
    }
}