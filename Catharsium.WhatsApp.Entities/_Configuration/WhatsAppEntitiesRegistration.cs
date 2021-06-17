using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.IO._Configuration;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Catharsium.WhatsApp.Data._Configuration
{
    public static class WhatsAppEntitiesRegistration
    {
        public static IServiceCollection AddWhatsAppEntities(this IServiceCollection services, IConfiguration config)
        {
            var configuration = config.Load<WhatsAppEntitiesSettings>();
            services.AddSingleton<WhatsAppEntitiesSettings, WhatsAppEntitiesSettings>(provider => configuration);

            services.AddIoUtilities(config);

            services.AddScoped<IEqualityComparer<Message>, MessageEqualityComparer>();
            services.AddScoped<IEqualityComparer<User>, UserEqualityComparer>();

            return services;
        }
    }
}