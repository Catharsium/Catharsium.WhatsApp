using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.IO.Console._Configuration;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Ui.Terminal.ActionHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catharsium.WhatsApp.Ui.Terminal._Configuration
{
    public static class WhatsAppTerminalUiRegistration
    {
        public static IServiceCollection AddWhatsAppTerminalUi(this IServiceCollection services, IConfiguration config)
        {
            var configuration = config.Load<WhatsAppTerminalUiSettings>();
            services.AddSingleton<WhatsAppTerminalUiSettings, WhatsAppTerminalUiSettings>(provider => configuration);

            services.AddConsoleIoUtilities(config);
            services.AddWhatsAppData(config);

            services.AddScoped<IActionHandler, ImportActionHandler>();
            services.AddScoped<IActionHandler, UsersActionHandler>(); 

            return services;
        }
    }
}