using Catharsium.Util.Testing;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Catharsium.WhatsApp.Data.Tests.Filters
{
    [TestClass]
    public class UserFilterTests : TestFixture<UserFilter>
    {
        #region Fixture

        private User User { get; set; }


        [TestInitialize]
        public void Initialize()
        {
            this.User = new User();
            this.SetDependency(this.User);
        }

        #endregion

        #region Includes

        [TestMethod]
        public void Includes_MessageWithExpectedUser_ReturnsTrue()
        {
            var message = new Message { Sender = this.User };
            var actual = this.Target.Includes(message);
            Assert.IsTrue(actual);
        }


        [TestMethod]
        public void Includes_MessageWithDifferentUser_ReturnsFalse()
        {
            var message = new Message { Sender = new User() };
            var actual = this.Target.Includes(message);
            Assert.IsFalse(actual);
        }

        #endregion
    }
}