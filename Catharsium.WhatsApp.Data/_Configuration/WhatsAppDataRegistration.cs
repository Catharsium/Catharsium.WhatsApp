using Catharsium.Util.Configuration.Extensions;
using Catharsium.WhatsApp.Entities.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catharsium.WhatsApp.Data._Configuration
{
    public static class WhatsAppDataRegistration
    {
        public static IServiceCollection AddWhatsAppData(this IServiceCollection services, IConfiguration config)
        {
            var configuration = config.Load<WhatsAppDataSettings>();
            services.AddSingleton<WhatsAppDataSettings, WhatsAppDataSettings>(provider => configuration);

            services.AddScoped<IWhatsAppExportFile, WhatsAppExportFile>();

            return services;
        }
    }
}