﻿using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.Testing.Extensions;
using Catharsium.WhatsApp.Terminal._Configuration;
using Catharsium.WhatsApp.Terminal.ActionHandlers;
using Catharsium.WhatsApp.Terminal.ActionHandlers.Steps;
using Catharsium.WhatsApp.Terminal.Data;
using Catharsium.WhatsApp.Terminal.Models;
using Catharsium.WhatsApp.Terminal.Terminal.Steps;
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
        serviceCollection.ReceivedRegistration<IActionHandler, UpdateUsersActionHandler>();
        serviceCollection.ReceivedRegistration<IActionHandler, ActivityListActionHandler>();
        serviceCollection.ReceivedRegistration<IActionHandler, NationalityActionHandler>();
        serviceCollection.ReceivedRegistration<IActionHandler, ActionHandler>();
        serviceCollection.ReceivedRegistration<IActionHandler, HourOfTheDayHistogramActionHandler>();
        serviceCollection.ReceivedRegistration<IActionHandler, DayOfTheWeekHistogramActionHandler>();

        serviceCollection.ReceivedRegistration<IConversationChooser, ConversationChooser>();
        serviceCollection.ReceivedRegistration<IPeriodChooser, PeriodChooser>();
    }


    [TestMethod]
    public void AddWhatsAppTerminal_RegistersPackages()
    {
        var serviceCollection = Substitute.For<IServiceCollection>();
        var configuration = Substitute.For<IConfiguration>();

        serviceCollection.AddWhatsAppTerminal(configuration);
        serviceCollection.ReceivedRegistration<IConversationsRepository>();
        serviceCollection.ReceivedRegistration<IEqualityComparer<Message>>();
    }
}