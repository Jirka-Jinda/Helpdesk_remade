using Database;
using Database.Context;
using Helpdesk.Filters;
using Microsoft.EntityFrameworkCore;
using Models.Users;
using Serilog;
using Services;

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

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

builder.Services.AddLogRetentionService(options =>
{
    options.LogDeletionInterval = TimeSpan.FromHours(24);
    options.LogDeleteFilesOdlerThan = TimeSpan.FromDays(7);
});

builder.Services.AddTicketArchiveService(options =>
{
    options.ArchiveTicketsInterval = TimeSpan.FromHours(1);
    options.ArchiveResolvedTicketsAfter = TimeSpan.FromDays(3);
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<TransactionFilter>();
    options.Filters.Add<ExceptionFilter>();
});

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.UseStatusCodePagesWithRedirects("/Error/Code{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

var supportedCultures = new[] { "cs-CZ" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("cs-CZ")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.Run();

public partial class Program;