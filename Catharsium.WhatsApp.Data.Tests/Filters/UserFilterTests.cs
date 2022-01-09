using Catharsium.Util.Testing;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
namespace Catharsium.WhatsApp.Data.Tests.Filters;

[TestClass]
public class UserFilterTests : TestFixture<UserFilter>
{
    #region Fixture

    private User User { get; set; }


    [TestInitialize]
    public void Initialize()
    {
        this.User = new User("+0123456789") {
            Aliases = new List<string> { "My alias" }
        };
        this.SetDependency(new[] { this.User });
    }

    #endregion

    #region Includes

    [TestMethod]
    public void Includes_SamePhoneNumber_ReturnsTrue()
    {
        var message = new Message { Sender = this.User.PhoneNumber };
        var actual = this.Target.Includes(message);
        Assert.IsTrue(actual);
    }


    [TestMethod]
    public void Includes_SameAlias_ReturnsTrue()
    {
        var message = new Message { Sender = this.User.Aliases[0] };
        var actual = this.Target.Includes(message);
        Assert.IsTrue(actual);
    }


    [TestMethod]
    public void Includes_DifferentPhoneNumber_ReturnsFalse()
    {
        var message = new Message { Sender = this.User.PhoneNumber + "Other" };
        var actual = this.Target.Includes(message);
        Assert.IsFalse(actual);
    }


    [TestMethod]
    public void Includes_DifferentAlias_ReturnsFalse()
    {
        var message = new Message { Sender = this.User.Aliases + "Other" };
        var actual = this.Target.Includes(message);
        Assert.IsFalse(actual);
    }

    #endregion
}