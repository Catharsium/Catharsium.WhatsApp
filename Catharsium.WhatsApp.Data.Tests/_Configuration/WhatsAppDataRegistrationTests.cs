using Catharsium.Util.Testing.Extensions;
using Catharsium.WhatsApp.Data;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Entities.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Catharsium.WhatsApp.Terminal.Tests._Configuration
{
    [TestClass]
    public class WhatsAppDataRegistrationTests
    {
        [TestMethod]
        public void AddWhatsAppData_RegistersDependencies()
        {
            var serviceCollection = Substitute.For<IServiceCollection>();
            var configuration = Substitute.For<IConfiguration>();

            serviceCollection.AddWhatsAppData(configuration);
            serviceCollection.ReceivedRegistration<IWhatsAppExportFile, WhatsAppExportFile>();
        }


        [TestMethod]
        public void AddWhatsAppData_RegistersPackages()
        {
            var serviceCollection = Substitute.For<IServiceCollection>();
            var configuration = Substitute.For<IConfiguration>();

            serviceCollection.AddWhatsAppData(configuration);
        }
    }
}