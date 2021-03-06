using Catharsium.Util.Testing;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Data.Repositories;
using Catharsium.WhatsApp.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
namespace Catharsium.WhatsApp.Data.Tests.Repositories;

[TestClass]
public class ActiveUsersRepositoryTests : TestFixture<ExportUsersRepository>
{
    #region Fixture

    private User AliasUser { get; set; }
    private User PhoneUser { get; set; }

    private static string ConversationName => "My conversation name";


    [TestInitialize]
    public void Initialize()
    {
        this.AliasUser = new User("My alias");
        this.PhoneUser = new User("+0123456789");

        var settings = new WhatsAppDataSettings {
            ActiveUsers = new Dictionary<string, string> {
                     { ConversationName , string.Join(", ", this.AliasUser.Aliases[0], this.PhoneUser.PhoneNumber) }
                 }
        };
        this.SetDependency(settings);
    }

    #endregion

    #region GetFor

    [TestMethod]
    public void GetFor_ValidConversationName_ReturnsUsers()
    {
        var actual = this.Target.GetForConversation(ConversationName);
        Assert.IsNotNull(actual);
        Assert.AreEqual(2, actual.Count);
    }


    [TestMethod]
    public void GetFor_InvalidConversationName_ReturnsEmptyList()
    {
        var actual = this.Target.GetForConversation(ConversationName + "Other");
        Assert.IsNotNull(actual);
        Assert.AreEqual(0, actual.Count);
    }


    [TestMethod]
    public void GetFor_PhoneNumberInList_ReturnsUserWithPhoneNumber()
    {
        var actual = this.Target.GetForConversation(ConversationName);
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any(u => u.PhoneNumber == this.PhoneUser.PhoneNumber));
    }


    [TestMethod]
    public void GetFor_AliasInList_ReturnsUserWithAlias()
    {
        var actual = this.Target.GetForConversation(ConversationName);
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any(u => u.Aliases.Any(a => a == this.AliasUser.Aliases[0])));
    }

    #endregion
}