using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.Filters;
using Catharsium.Util.IO._Configuration;
using Catharsium.Util.IO.Interfaces;
using Catharsium.Util.IO.Json;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Data.Repository;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
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

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IWhatsAppRepository, WhatsAppRepository>();
            services.AddScoped<IFilter<Message>, PeriodFilter>();
            services.AddScoped<IFilter<Message>, UserFilter>();

            return services;
        }
    }
}