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


        // Send complaint text + image to Gemini
        string aiResult =
            await _geminiService.AnalyzeComplaint(
                description,
                image
            );


        // Default values
        string category = "Other";
        string priority = "Low";


        // AI Category
        if (aiResult.Contains("Category: Road"))
        {
            category = "Road";
        }
        else if (aiResult.Contains("Category: Water"))
        {
            category = "Water";
        }
        else if (aiResult.Contains("Category: Electricity"))
        {
            category = "Electricity";
        }
        else if (aiResult.Contains("Category: Garbage"))
        {
            category = "Garbage";
        }


        // AI Priority
        if (aiResult.Contains("Priority: High"))
        {
            priority = "High";
        }
        else if (aiResult.Contains("Priority: Medium"))
        {
            priority = "Medium";
        }


        // Create complaint
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
                "There is a large pothole on the main road."
            );


        return Ok(result);
    }
}