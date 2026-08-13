using Google.GenAI;
using Google.GenAI.Types;

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
        Analyze this city complaint.

        Complaint:
        {description}

        If an image is provided, inspect the image and use it
        together with the complaint description.

        Return only:

        Detected Issue: [short description]
        Category: [Road/Water/Electricity/Garbage/Other]
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