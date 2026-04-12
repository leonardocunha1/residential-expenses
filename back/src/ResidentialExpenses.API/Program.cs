using ResidentialExpenses.API.Filters;
using ResidentialExpenses.API.Token;
using ResidentialExpenses.Application;
using ResidentialExpenses.Domain.Security.Tokens;
using ResidentialExpenses.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Services.ApplyMigrations();
app.Run();
