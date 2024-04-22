using Microsoft.AspNetCore.Builder;

namespace Template.Api;

public static class TemplatesApi
{
    public static void MapTemplates(this WebApplication app)
    {
        var group = app.MapGroup("templates");

        group.WithTags("Templates");

        group.MapGet("/", () =>
        {
            return TypedResults.Ok("Hello, Templates!");
        })
        .WithName("GeTemplates")
        .WithOpenApi();

        group.MapPost("/files", async (IFormFile file) =>
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var content = await reader.ReadToEndAsync();

            return TypedResults.Ok(new { Length = content.Length });
        })
        .WithName("UploadTemplatesFile")
        .DisableAntiforgery();
    }
}
