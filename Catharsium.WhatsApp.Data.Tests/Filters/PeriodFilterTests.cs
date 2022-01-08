using Catharsium.Util.Testing;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Terminal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace Catharsium.WhatsApp.Data.Tests.Filters;

[TestClass]
public class PeriodFilterTests : TestFixture<PeriodFilter>
{
    #region Fixture

    private DateTime From { get; set; }
    private DateTime To { get; set; }


    [TestInitialize]
    public void Initialize()
    {
        this.To = DateTime.Today;
        this.From = this.To.AddDays(-7);
        this.SetDependency(new Period {
            From = this.From,
            To = this.To
        });
    }

    #endregion

    #region Includes

    [TestMethod]
    public void Includes_TimestampBeforeFrom_ReturnsFalse()
    {
        var message = new Message { Timestamp = this.From.AddDays(-1) };
        var actual = this.Target.Includes(message);
        Assert.IsFalse(actual);
    }


    [TestMethod]
    public void Includes_TimestampBetweenFromAndTo_ReturnsTrue()
    {
        var message = new Message { Timestamp = this.To.AddDays(-1) };
        var actual = this.Target.Includes(message);
        Assert.IsTrue(actual);
    }


    [TestMethod]
    public void Includes_TimestampAfterTo_ReturnsFalse()
    {
        var message = new Message { Timestamp = this.To.AddDays(1) };
        var actual = this.Target.Includes(message);
        Assert.IsFalse(actual);
    }

    #endregion
}