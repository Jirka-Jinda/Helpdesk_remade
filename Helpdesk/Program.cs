using Database;
using Database.Context;
using Helpdesk.Filters;
using Microsoft.EntityFrameworkCore;
using Models.Users;
using Serilog;
using Services;
using Services.Options;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Database")));

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Access/Login";
    options.LogoutPath = "/Access/Logout";
    options.AccessDeniedPath = "/Error/Code403";
});

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddEmailNotificationService(builder.Configuration);

builder.Services.Configure<LogRetentionOptions>(builder.Configuration.GetSection("LogRetentionOptions"));
builder.Services.AddLogRetentionService(builder.Configuration);

builder.Services.Configure<TicketArchiveOptions>(builder.Configuration.GetSection("TicketArchiveOptions"));
builder.Services.AddTicketArchiveService(builder.Configuration);

builder.Services.Configure<TicketAssignmentOptions>(builder.Configuration.GetSection("TicketAssignmentOptions"));
builder.Services.AddAutomaticAssignmentService(builder.Configuration);

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
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

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