using Microsoft.AspNetCore.Builder;

namespace Template.Api;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("templates");

        group.MapGet("/", () =>
        {
            return Results.Text("Hello, Templates!");
        })
        .WithName("GeTemplates")
        .WithOpenApi();

        group.MapPost("/files", async (IFormFile file) =>
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var content = await reader.ReadToEndAsync();

            return Results.Ok(new { Length = content.Length });
        })
        .WithName("UploadTemplatesFile")
        .DisableAntiforgery();
    }
}
