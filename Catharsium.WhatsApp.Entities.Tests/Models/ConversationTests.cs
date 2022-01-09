using Catharsium.Util.Testing;
using Catharsium.WhatsApp.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
namespace Catharsium.WhatsApp.Entities.Tests.Models;

[TestClass]
public class ConversationTests : TestFixture<Conversation>
{
    #region Name

    [TestMethod]
    public void Name_WithoutPrefix_IsStoredUnchanged()
    {
        var name = "My name";
        this.Target.Name = "WhatsApp Chat with " + name;
        Assert.AreEqual(name, this.Target.Name);
    }


    [TestMethod]
    public void Name_WithPrefix_IsStoredWithoutPrefix()
    {
        var name = "My name";
        this.Target.Name = "WhatsApp Chat with " + name;
        Assert.AreEqual(name, this.Target.Name);
    }

    #endregion

    #region ToString

    [TestMethod]
    public void ToString_ReturnsExpected()
    {
        this.Target.Name = "My name";
        this.Target.Messages = new List<Message> {
                new Message(),
                new Message(),
                new Message()
            };

        var actual = this.Target.ToString();
        Assert.AreEqual($"{this.Target.Name} ({this.Target.Messages.Count} messages)", actual);
    }

    #endregion
}