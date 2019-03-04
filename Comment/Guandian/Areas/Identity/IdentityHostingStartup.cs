using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Guandian.Areas.Identity.IdentityHostingStartup))]
namespace Guandian.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}