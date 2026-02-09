using DotNetEnv; // dotnet connection 
using Microsoft.EntityFrameworkCore;
using SuperShop.Data; // database connection

var builder = WebApplication.CreateBuilder(args);

// using for env variable file calling
Env.Load();

// Add services to the container.
builder.Services.AddControllersWithViews();

// database connection
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
   );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=Index}/{id?}");

app.Run();
