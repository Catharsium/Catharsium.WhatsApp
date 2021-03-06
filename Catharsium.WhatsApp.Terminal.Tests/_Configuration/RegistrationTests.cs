using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Catharsium.Util.Testing.Extensions;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Terminal._Configuration;
using Catharsium.WhatsApp.Terminal.ActionHandlers;
using Catharsium.WhatsApp.Terminal.ActionHandlers.Steps;
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
    public void AddWhatsAppTerminal_RegistersDependencies()
    {
        var serviceCollection = Substitute.For<IServiceCollection>();
        var configuration = Substitute.For<IConfiguration>();

        serviceCollection.AddWhatsAppTerminal(configuration);
        serviceCollection.ReceivedRegistration<IActionHandler, ImportMessagesActionHandler>();
        serviceCollection.ReceivedRegistration<IActionHandler, ImportUsersActionHandler>();
        serviceCollection.ReceivedRegistration<IActionHandler, ActivityListActionHandler>();
        serviceCollection.ReceivedRegistration<IActionHandler, NationalityActionHandler>();
        serviceCollection.ReceivedRegistration<IActionHandler, JokeActionHandler>();
        serviceCollection.ReceivedRegistration<IActionHandler, HourOfTheDayHistogramActionHandler>();
        serviceCollection.ReceivedRegistration<IActionHandler, DayOfTheWeekHistogramActionHandler>();

        serviceCollection.ReceivedRegistration<ISelectionActionStep<Conversation>, ConversationChooser>();
        serviceCollection.ReceivedRegistration<ISelectionActionStep<Period>, PeriodChooser>();
    }


    [TestMethod]
    public void AddWhatsAppTerminal_RegistersPackages()
    {
        var serviceCollection = Substitute.For<IServiceCollection>();
        var configuration = Substitute.For<IConfiguration>();

        serviceCollection.AddWhatsAppTerminal(configuration);
        serviceCollection.ReceivedRegistration<IExportFilesRepository>();
        serviceCollection.ReceivedRegistration<IEqualityComparer<Message>>();
    }
}