using DawnWeaver.Application;
using DawnWeaver.Infrastructure;
using Microsoft.OpenApi;
using DawnWeaver.Persistance;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();

builder.Logging.AddSerilog(logger);

builder.Services.AddCors(options =>
    options.AddPolicy("MyAllowSpecificOrigins",
        configure =>
        {
            configure.WithOrigins("http://localhost:4200"); // To be changed when frontend is deployed
        }));

builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DawnWeaver",
        Version = "v1",
        Description = "Time management and productivity app",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Jakub Woźniak",
            Email = "email@email.com",
            Url = new Uri("https://example.com")
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license")
        }
    });
    var filePath = Path.Combine(AppContext.BaseDirectory, "DawnWeaver.xml");
    c.IncludeXmlComments(filePath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DawnWeaver"));
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();