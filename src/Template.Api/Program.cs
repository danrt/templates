using Microsoft.AspNetCore.Diagnostics;
using OwaspHeaders.Core.Extensions;
using Serilog;
using Template.Api;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseIISIntegration();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpLogging(c =>
{
    c.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestBody
        | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPath
        | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestMethod;
    c.CombineLogs = true;
});

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

        await Results.Problem(exceptionHandlerFeature?.Error.Message).ExecuteAsync(context);
    }));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseSecureHeadersMiddleware();

    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
