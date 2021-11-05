using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.Filters;
using Catharsium.Util.IO._Configuration;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Data.Logic;
using Catharsium.WhatsApp.Data.Repositories.Readers;
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

            services.AddScoped<IActiveUsersRepository, ActiveUsersRepository>();
            services.AddScoped<IConversationsRepository, ConversationsRepository>();
            services.AddScoped<IConversationUsersRepository, UsersRepository>();

            services.AddScoped<IMessageParser, MessageParser>();

            services.AddScoped<IMessageAnalyzer, MessageAnalyzer>();

            services.AddScoped<IFilter<Message>, PeriodFilter>();
            services.AddScoped<IFilter<Message>, UserFilter>();

            return services;
        }
    }
}