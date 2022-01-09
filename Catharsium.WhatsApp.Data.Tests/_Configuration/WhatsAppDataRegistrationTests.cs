using Catharsium.Util.Filters;
using Catharsium.Util.IO.Interfaces;
using Catharsium.Util.Testing.Extensions;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Data.Repositories;
using Catharsium.WhatsApp.Data.Repositories.Readers;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
namespace Catharsium.WhatsApp.Data.Tests._Configuration;

[TestClass]
public class WhatsAppDataRegistrationTests
{
    [TestMethod]
    public void AddWhatsAppData_RegistersDependencies()
    {
        var serviceCollection = Substitute.For<IServiceCollection>();
        var configuration = Substitute.For<IConfiguration>();

        serviceCollection.AddWhatsAppData(configuration);
        serviceCollection.ReceivedRegistration<IActiveUsersRepository, ActiveUsersRepository>();
        serviceCollection.ReceivedRegistration<IExportFilesRepository, ExportFilesRepository>();
        serviceCollection.ReceivedRegistration<IConversationUsersRepository, UsersRepository>();

        serviceCollection.ReceivedRegistration<IMessageParser, MessageParser>();

        serviceCollection.ReceivedRegistration<IFilter<Message>, PeriodFilter>();
        serviceCollection.ReceivedRegistration<IFilter<Message>, UserFilter>();
    }


    [TestMethod]
    public void AddWhatsAppData_RegistersPackages()
    {
        var serviceCollection = Substitute.For<IServiceCollection>();
        var configuration = Substitute.For<IConfiguration>();

        serviceCollection.AddWhatsAppData(configuration);
        serviceCollection.ReceivedRegistration<IFileFactory>();
    }
}