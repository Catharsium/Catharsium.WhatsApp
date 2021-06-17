using Catharsium.Math.Graph._Configuration;
using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.IO.Console._Configuration;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
using Catharsium.WhatsApp.Terminal.ActionHandlers;
using Catharsium.WhatsApp.Terminal.ActionHandlers.Basic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catharsium.WhatsApp.Ui.Terminal._Configuration
{
    public static class WhatsAppTerminalRegistration
    {
        public static IServiceCollection AddWhatsAppTerminal(this IServiceCollection services, IConfiguration config)
        {
            var configuration = config.Load<WhatsAppTerminalSettings>();
            services.AddSingleton<WhatsAppTerminalSettings, WhatsAppTerminalSettings>(provider => configuration);

            services.AddConsoleIoUtilities(config);
            services.AddGraphMath(config);
            services.AddWhatsAppData(config);
            services.AddWhatsAppEntities(config);

            services.AddScoped<IActionHandler, ImportActiveUsersActionHandler>();
            services.AddScoped<IActionHandler, ActivityListActionHandler>();
            services.AddScoped<IActionHandler, NationalityActionHandler>();
            services.AddScoped<IActionHandler, ActionHandler>(); 
            services.AddScoped<IActionHandler, MessagesActionHandler>();
            services.AddScoped<IActionHandler, HourOfTheDayHistogramActionHandler>();
            services.AddScoped<IActionHandler, DayOfTheWeekHistogramActionHandler>();

            services.AddScoped<IConversationChooser, ConversationChooser>();
            services.AddScoped<IPeriodChooser, PeriodChooser>();

            return services;
        }
    }
}