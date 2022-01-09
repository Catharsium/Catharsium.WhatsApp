using Catharsium.Util.Testing;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace Catharsium.WhatsApp.Entities.Tests.Models.Comparers;

[TestClass]
public class MessageEqualityComparerTests : TestFixture<MessageEqualityComparer>
{
    #region Equals

    [TestMethod]
    public void Equals_ObjectAndItself_ReturnsTrue()
    {
        var @object = new Message {
            Timestamp = DateTime.Now,
            Sender = "My sender",
            Text = "My text"
        };

        var actual = this.Target.Equals(@object, @object);
        Assert.IsTrue(actual);
    }


    [TestMethod]
    public void Equals_NullAndObject_ReturnsFalse()
    {
        var @object = new Message {
            Timestamp = DateTime.Now,
            Sender = "My sender",
            Text = "My text"
        };

        var actual = this.Target.Equals(null, @object);
        Assert.IsFalse(actual);
    }


    [TestMethod]
    public void Equals_ObjectAndNull_ReturnsFalse()
    {
        var @object = new Message {
            Timestamp = DateTime.Now,
            Sender = "My sender",
            Text = "My text"
        };

        var actual = this.Target.Equals(@object, null);
        Assert.IsFalse(actual);
    }


    [TestMethod]
    public void Equals_ObjectAndACopy_ReturnsTrue()
    {
        var @object = new Message {
            Timestamp = DateTime.Now,
            Sender = "My sender",
            Text = "My text"
        };
        var other = new Message {
            Timestamp = @object.Timestamp,
            Sender = @object.Sender,
            Text = @object.Text
        };

        var actual = this.Target.Equals(@object, other);
        Assert.IsTrue(actual);
    }


    [TestMethod]
    public void Equals_DifferentTimestamp_ReturnsFalse()
    {
        var @object = new Message {
            Timestamp = DateTime.Now,
            Sender = "My sender",
            Text = "My text"
        };
        var other = new Message {
            Timestamp = @object.Timestamp.AddSeconds(-1),
            Sender = @object.Sender,
            Text = @object.Text
        };

        var actual = this.Target.Equals(@object, other);
        Assert.IsFalse(actual);
    }


    [TestMethod]
    public void Equals_DifferentSender_ReturnsFalse()
    {
        var @object = new Message {
            Timestamp = DateTime.Now,
            Sender = "My sender",
            Text = "My text"
        };
        var other = new Message {
            Timestamp = @object.Timestamp,
            Sender = @object.Sender + "Other",
            Text = @object.Text
        };

        var actual = this.Target.Equals(@object, other);
        Assert.IsFalse(actual);
    }


    [TestMethod]
    public void Equals_DifferentText_ReturnsFalse()
    {
        var @object = new Message {
            Timestamp = DateTime.Now,
            Sender = "My sender",
            Text = "My text"
        };
        var other = new Message {
            Timestamp = @object.Timestamp,
            Sender = @object.Sender,
            Text = @object.Text + "Other"
        };

        var actual = this.Target.Equals(@object, other);
        Assert.IsFalse(actual);
    }

    #endregion

    #region GetHashCode

    [TestMethod]
    public void GetHashCode_ValidObject_ReturnsHashCodeOfObject()
    {
        var @object = new Message {
            Timestamp = DateTime.Now,
            Sender = "My sender",
            Text = "My text"
        };
        var expected = @object.Text.GetHashCode();

        var actual = this.Target.GetHashCode(@object);
        Assert.AreEqual(expected, actual);
    }

    #endregion
}