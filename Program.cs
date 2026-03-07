using DotNetEnv; // dotnet connection 
using Microsoft.EntityFrameworkCore;
using SuperShop.Data; // database connection

var builder = WebApplication.CreateBuilder(args);

// using for env variable file calling
Env.Load();

// Add services to the container.
builder.Services.AddControllersWithViews();

// use for create session and session time set
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
});

// use session in navbar header section
builder.Services.AddHttpContextAccessor();

// database connection
var connectionString = $"server={Environment.GetEnvironmentVariable("DB_SERVER")};port={Environment.GetEnvironmentVariable("DB_PORT")};database={Environment.GetEnvironmentVariable("DB_NAME")};user={Environment.GetEnvironmentVariable("DB_USER")};password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

// use seesion
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=Index}/{id?}");

app.Run();
