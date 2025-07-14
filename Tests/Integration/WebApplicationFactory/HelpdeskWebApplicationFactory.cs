using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Tests.Integration.WebApplicationFactory;

public class HelpdeskWebApplicationFactory : WebApplicationFactory<Program>
{
    static HelpdeskWebApplicationFactory()
    {
        Environment.SetEnvironmentVariable("DOTNET_hostBuilder:reloadConfigOnChange", "false");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseConfiguration(new ConfigurationBuilder()
            .Build());
        builder.UseEnvironment("Test");
    }
}
