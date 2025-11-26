using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SCADashboard.Application.Services;
using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using SCADashboard.Infrastructure.Helper;
using SCADashboard.Infrastructure.Repositories;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Logging Action Filter globally
//builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<LoggingActionFilter>();
});


//EF Core Context Registration
builder.Services.AddDbContext<SCADashboardDbContext>(options=>
    options.UseSqlServer(ClientInfoHelper.Decryption(builder.Configuration.GetConnectionString("DefaultConnection")),
    sqlOptions => sqlOptions.EnableRetryOnFailure()));


////EF Core Context Registration
//builder.Services.AddDbContext<SCADashboardDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
//    sqlOptions => sqlOptions.EnableRetryOnFailure(
//        maxRetryCount: 1,          // number of retry attempts
//        maxRetryDelay: TimeSpan.FromSeconds(10), // delay between retries
//        errorNumbersToAdd: null    // additional error numbers to consider transient
//    )));

// Serilog Configuration for File Logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

//// Add localization services
//builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

//// Configure supported cultures
//var supportedCultures = new[]
//{
//    new CultureInfo("en-US"),
//    new CultureInfo("de-DE"),
//    new CultureInfo("fr-FR")
//};

//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    options.DefaultRequestCulture = new RequestCulture("en-US");
//    options.SupportedCultures = supportedCultures;
//    options.SupportedUICultures = supportedCultures;

//    // Detect culture from browser headers
//    options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
//});


//Dependency Injection for Repositories
// Generic repository
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Login Module
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();

// User
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Role
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Kunde
builder.Services.AddScoped<IKundeRepository, KundeRepository>();
builder.Services.AddScoped<IKundeService, KundeService>();

// Audit
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IAuditService, AuditService>();

// Password Log
builder.Services.AddScoped<IPasswordLogRepository, PasswordLogRepository>();
builder.Services.AddScoped<IPasswordLogService, PasswordLogService>();

// <-- Add this line For Session Access in PageAuthorizeAttribute
builder.Services.AddHttpContextAccessor(); 

//Session Configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Authentication Configuration
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Login/Index";
//        options.LogoutPath = "/Login/Logout";
//        options.AccessDeniedPath = "/Home/AccessDenied";
//    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Enable Session Middleware
app.UseSession();

//app.Use(async (context, next) =>
//{
//    await next();

//    // Only apply to authenticated pages
//    if (context.User.Identity?.IsAuthenticated == true)
//    {
//        context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
//        context.Response.Headers["Pragma"] = "no-cache";
//        context.Response.Headers["Expires"] = "0";
//    }
//});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Enable localization
//var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
//app.UseRequestLocalization(localizationOptions);


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthentication(); // ✅ Must be before UseAuthorization
app.UseAuthorization();

// Your endpoints
app.MapControllerRoute(
    name: "Login",
    pattern: "",
    defaults: new { controller = "login", action = "Index" }
);

// Your endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

//web Application Exception Handling
app.UseExceptionHandler("/Error/Error404");


// Custom 404 handling middleware
//app.Use(async (context, next) =>
//{
//    await next();

//    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
//    {
//        // Redirect to general page
//        context.Request.Path = "/Home/General"; // <-- your general page
//        context.Response.StatusCode = 200;      // reset status code
//        await next(); // invoke the pipeline again for the new path
//    }
//});

app.Run();
