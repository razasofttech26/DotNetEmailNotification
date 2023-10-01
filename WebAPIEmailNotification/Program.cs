using Microsoft.Extensions.Configuration;
using WebAPIEmailNotification.Interfaces;
using WebAPIEmailNotification.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowURL", builder =>
    {
        builder.WithOrigins(
                "http://127.0.0.1:5502",   // Allow requests from your local URL
                "https://razasofttech.com" // Allow requests from your live URL
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowURL");
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
