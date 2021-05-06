using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.IO._Configuration;
using Catharsium.WhatsApp.Data.Repository;
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

            services.AddIoUtilities(config);

            services.AddScoped<IWhatsAppRepository, WhatsAppRepository>();

            return services;
        }
    }
}