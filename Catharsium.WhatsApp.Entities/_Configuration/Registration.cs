using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.IO.Files._Configuration;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Catharsium.WhatsApp.Terminal._Configuration;

public static class Registration
{
    public static IServiceCollection AddWhatsAppEntities(this IServiceCollection services, IConfiguration config)
    {
        var configuration = config.Load<WhatsAppEntitiesSettings>();
        services.AddSingleton<WhatsAppEntitiesSettings, WhatsAppEntitiesSettings>(provider => configuration);

        services.AddFilesIoUtilities(config);

        services.AddScoped<IEqualityComparer<Message>, MessageEqualityComparer>();
        services.AddScoped<IEqualityComparer<User>, UserEqualityComparer>();

        return services;
    }
}