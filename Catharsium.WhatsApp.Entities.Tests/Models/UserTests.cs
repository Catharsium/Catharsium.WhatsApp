using Catharsium.Util.Testing;
using Catharsium.WhatsApp.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Catharsium.WhatsApp.Entities.Tests.Models
{
    [TestClass]
    public class UserTests : TestFixture<User>
    {
        [TestMethod]
        public void Constructor_User_CopiesAllProperties()
        {
            var user = new User {
                PhoneNumber = "My phone number",
                DisplayName = "My display name",
                Aliases = new List<string> { "My alias 1", "My alias 2", "My alias 3" },
                IsActive = true
            };

            var actual = new User(user);
            Assert.AreNotEqual(user, actual);
            Assert.AreEqual(user.PhoneNumber, actual.PhoneNumber);
            Assert.AreEqual(user.DisplayName, actual.DisplayName);
            Assert.AreEqual(user.Aliases.Count, actual.Aliases.Count);
            foreach (var alias in user.Aliases) {
                Assert.IsTrue(actual.Aliases.Contains(alias));
            }
            Assert.AreEqual(user.IsActive, actual.IsActive);
        }
    }
}