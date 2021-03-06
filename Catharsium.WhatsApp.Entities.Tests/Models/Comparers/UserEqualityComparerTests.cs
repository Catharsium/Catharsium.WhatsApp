using Catharsium.Util.Testing;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
namespace Catharsium.WhatsApp.Entities.Tests.Models.Comparers;

[TestClass]
public class UserEqualityComparerTests : TestFixture<UserEqualityComparer>
{
    #region Fixture

    private User User { get; set; }


    [TestInitialize]
    public void Initialize()
    {
        this.User = new User("+0123456789") {
            Aliases = new List<string> { "My alias 1", "My alias 2", "My alias 3" },
            DisplayName = "My display name",
            Conversations = new List<string> { "My conversation 1", "My conversation 2", "My conversation 3" }
        };
    }

    #endregion

    #region Equals

    [TestMethod]
    public void Equals_ObjectAndItself_ReturnsTrue()
    {
        var actual = this.Target.Equals(this.User, this.User);
        Assert.IsTrue(actual);
    }


    [TestMethod]
    public void Equals_NullAndObject_ReturnsFalse()
    {
        var actual = this.Target.Equals(null, this.User);
        Assert.IsFalse(actual);
    }


    [TestMethod]
    public void Equals_ObjectAndNull_ReturnsFalse()
    {
        var actual = this.Target.Equals(this.User, null);
        Assert.IsFalse(actual);
    }


    [TestMethod]
    public void Equals_ObjectAndACopy_ReturnsTrue()
    {
        var other = new User(this.User.PhoneNumber) {
            Aliases = new List<string>(this.User.Aliases),
            DisplayName = this.User.DisplayName,
            Conversations = new List<string>(this.User.Conversations)
        };

        var actual = this.Target.Equals(this.User, other);
        Assert.IsTrue(actual);
    }


    [TestMethod]
    public void Equals_DifferentPhoneNumber_ReturnsFalse()
    {
        var other = new User(this.User.PhoneNumber + "Other") {
            Aliases = new List<string>(this.User.Aliases),
            DisplayName = this.User.DisplayName,
            Conversations = new List<string>(this.User.Conversations)
        };

        var actual = this.Target.Equals(this.User, other);
        Assert.IsFalse(actual);
    }

    #endregion

    #region GetHashCode

    [TestMethod]
    public void GetHashCode_ValidObject_ReturnsHashCodeOfObject()
    {
        var expected = this.User.PhoneNumber.GetHashCode();
        var actual = this.Target.GetHashCode(this.User);
        Assert.AreEqual(expected, actual);
    }

    #endregion
}