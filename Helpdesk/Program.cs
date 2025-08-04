using Database;
using Database.Context;
using Helpdesk.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Users;
using Serilog;
using Services;
using Services.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.override.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .WriteTo.Console()
#else
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
#endif
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

#if DEBUG
var connectionString = builder.Configuration.GetConnectionString("TestingConnection") 
    ?? throw new InvalidOperationException("Connection string 'TestingConnection' not found.");
#else
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
#endif

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Database")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ěščřžýáíéúůňďťĚŠČŘŽÝÁÍÉÚŮŇĎŤ ";
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;

    options.LoginPath = "/Access/Login";
    options.LogoutPath = "/Access/Logout";
    options.AccessDeniedPath = "/Error/Code403";
});

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.Configure<EmailNotificationsOptions>(builder.Configuration.GetSection("EmailNotificationsOptions"));
builder.Services.AddEmailNotificationService(builder.Configuration);

builder.Services.Configure<LogRetentionOptions>(builder.Configuration.GetSection("LogRetentionOptions"));
builder.Services.AddLogRetentionService(builder.Configuration);

builder.Services.Configure<TicketArchiveOptions>(builder.Configuration.GetSection("TicketArchiveOptions"));
builder.Services.AddTicketArchiveService(builder.Configuration);

builder.Services.Configure<TicketAssignmentOptions>(builder.Configuration.GetSection("TicketAssignmentOptions"));
builder.Services.AddAutomaticAssignmentService(builder.Configuration);

builder.Services.Configure<RegistrationOptions>(builder.Configuration.GetSection("RegistrationOptions"));

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<TransactionFilter>();
    options.Filters.Add<ExceptionFilter>();
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddMemoryCache(options => 
{
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(15);
});

builder.Services.AddSession(options =>
{ 
    options.IdleTimeout = TimeSpan.FromMinutes(15);
});

builder.Services.AddRazorPages();

var app = builder.Build();

await app.Services.ApplyMigrationsAsync(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseHsts();
    app.UseStatusCodePagesWithRedirects("/Error/Code{0}");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");
app.MapRazorPages();

var supportedCultures = new[] { "cs-CZ" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("cs-CZ")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.Run();

public partial class Program;