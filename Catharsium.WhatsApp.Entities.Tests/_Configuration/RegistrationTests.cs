using Catharsium.Util.IO.Interfaces;
using Catharsium.Util.Testing.Extensions;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;
using Catharsium.WhatsApp.Terminal._Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
namespace Catharsium.WhatsApp.Terminal.Tests._Configuration;

[TestClass]
public class RegistrationTests
{
    [TestMethod]
    public void AddWhatsAppEntities_RegistersDependencies()
    {
        var serviceCollection = Substitute.For<IServiceCollection>();
        var configuration = Substitute.For<IConfiguration>();

        serviceCollection.AddWhatsAppEntities(configuration);
        serviceCollection.ReceivedRegistration<IEqualityComparer<Message>, MessageEqualityComparer>();
        serviceCollection.ReceivedRegistration<IEqualityComparer<User>, UserEqualityComparer>();
    }


    [TestMethod]
    public void AddWhatsAppEntities_RegistersPackages()
    {
        var serviceCollection = Substitute.For<IServiceCollection>();
        var configuration = Substitute.For<IConfiguration>();

        serviceCollection.AddWhatsAppEntities(configuration);
        serviceCollection.ReceivedRegistration<IFileFactory>();
    }
}