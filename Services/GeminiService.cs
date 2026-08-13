using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.AspNetCore.Http;

namespace SmartCityComplaint.Services;

public class GeminiService
{
    private readonly Client _client;

    public GeminiService()
    {
        string apiKey =
            System.Environment.GetEnvironmentVariable("GEMINI_API_KEY")
            ?? throw new InvalidOperationException(
                "GEMINI_API_KEY environment variable is not set.");

        _client = new Client(apiKey: apiKey);
    }

    public async Task<string> AnalyzeComplaint(
        string description,
        IFormFile? image = null)
    {
        string prompt = $"""
        Analyze this civic complaint.

        Complaint:
        {description}

        If an image is provided, inspect it and use it together
        with the complaint description.

        Identify the most specific and practical civic issue category.
        Do not restrict yourself to a fixed list.

        Use a category that clearly helps an administrator decide
        which department should handle the complaint.

        Examples:
        Road
        Water Supply
        Electricity
        Garbage
        Drainage
        Traffic Signal
        Fire & Emergency
        Public Safety
        Animal Control
        Pollution
        Public Transport
        Streetlight
        Other

        For fire-related complaints, prefer "Fire & Emergency".
        For traffic-signal complaints, prefer "Traffic Signal".
        For potholes or damaged roads, prefer "Road".
        For water leakage or supply issues, prefer "Water Supply".
        For garbage or waste issues, prefer "Garbage".
        For drainage problems, prefer "Drainage".
        For streetlight problems, prefer "Streetlight".

        Return only:

        Detected Issue: [short description]
        Category: [most specific civic issue category]
        Priority: [Low/Medium/High]
        """;

        var parts = new List<Part>
        {
            new Part
            {
                Text = prompt
            }
        };

        if (image != null && image.Length > 0)
        {
            using var memoryStream = new MemoryStream();

            await image.CopyToAsync(memoryStream);

            parts.Add(
                new Part
                {
                    InlineData = new Blob
                    {
                        MimeType = image.ContentType,
                        Data = memoryStream.ToArray()
                    }
                }
            );
        }

        var contents = new List<Content>
        {
            new Content
            {
                Role = "user",
                Parts = parts
            }
        };

        var response =
            await _client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash",
                contents: contents
            );

        return response
            .Candidates?[0]
            .Content?
            .Parts?[0]
            .Text ?? "";
    }
}