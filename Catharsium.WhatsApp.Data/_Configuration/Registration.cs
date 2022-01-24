using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.Interfaces;
using Catharsium.Util.IO.Files._Configuration;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Data.Interfaces;
using Catharsium.WhatsApp.Data.Logic;
using Catharsium.WhatsApp.Data.Repositories;
using Catharsium.WhatsApp.Data.Repositories.Readers;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Catharsium.WhatsApp.Data._Configuration;

public static class Registration
{
    public static IServiceCollection AddWhatsAppData(this IServiceCollection services, IConfiguration config)
    {
        var configuration = config.Load<WhatsAppDataSettings>();
        services.AddSingleton<WhatsAppDataSettings, WhatsAppDataSettings>(provider => configuration);

        services.AddFilesIoUtilities(config);

        services.AddScoped<IExportUsersRepository, ExportUsersRepository>();
        services.AddScoped<IExportFilesRepository, ExportFilesRepository>();
        services.AddScoped<IConversationsRepository, ConversationsRepository>();
        services.AddScoped<IConversationUsersRepository, ConversationUsersRepository>();

        services.AddScoped<IMessageAnalyzer, MessageAnalyzer>();
        services.AddScoped<IMessageParser, MessageParser>();

        services.AddScoped<IFilter<Message>, PeriodFilter>();
        services.AddScoped<IFilter<Message>, UserFilter>();

        return services;
    }
}