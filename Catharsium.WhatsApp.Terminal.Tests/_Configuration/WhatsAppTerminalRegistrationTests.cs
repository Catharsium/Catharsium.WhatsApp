using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.Testing.Extensions;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Ui.Terminal._Configuration;
using Catharsium.WhatsApp.Ui.Terminal.ActionHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Catharsium.WhatsApp.Terminal.Tests._Configuration
{
    [TestClass]
    public class WhatsAppTerminalRegistrationTests
    {
        [TestMethod]
        public void AddWhatsAppTerminal_RegistersDependencies()
        {
            var serviceCollection = Substitute.For<IServiceCollection>();
            var configuration = Substitute.For<IConfiguration>();

            serviceCollection.AddWhatsAppTerminal(configuration);
            serviceCollection.ReceivedRegistration<IActionHandler, ImportActionHandler>();
            serviceCollection.ReceivedRegistration<IActionHandler, UsersActionHandler>();

        }


        [TestMethod]
        public void AddWhatsAppTerminal_RegistersPackages()
        {
            var serviceCollection = Substitute.For<IServiceCollection>();
            var configuration = Substitute.For<IConfiguration>();

            serviceCollection.AddWhatsAppTerminal(configuration);
            serviceCollection.ReceivedRegistration<IWhatsAppExportFile>();
        }
    }
}