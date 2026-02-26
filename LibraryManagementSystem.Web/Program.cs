using LibraryManagementSystem.Persistence;
using LibraryManagementSystem.Services.Services;
using LibraryManagementSystem.Web.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbContext"));
});

builder.Services
    .AddScoped<BookService>()
    .AddScoped<IssueService>()
    .AddScoped<CategoryService>()
    .AddScoped<MemberService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();
var apiGroup = app.MapGroup("api");
// /api/master/books
apiGroup.MapIssueEndpoints()
    .MapBookEndpoints()
    .MapCategoryEndpoints()
    .MapMemberEndpoints();

app.MapGet("/", () => $"Running in {app.Environment.EnvironmentName} right now.");

app.Run();