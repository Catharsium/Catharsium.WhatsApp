using Catharsium.Util.Testing;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Catharsium.WhatsApp.Entities.Tests.Models.Comparers
{
    [TestClass]
    public class UserEqualityComparerTests : TestFixture<UserEqualityComparer>
    {
        #region Equals

        [TestMethod]
        public void Equals_ObjectAndItself_ReturnsTrue()
        {
            var @object = new User {
                Aliases = new List<string> { "My alias 1", "My alias 2", "My alias 3" },
                DisplayName = "My display name",
                IsActive = true,
                PhoneNumber = "My phone number"
            };

            var actual = this.Target.Equals(@object, @object);
            Assert.IsTrue(actual);
        }


        [TestMethod]
        public void Equals_NullAndObject_ReturnsFalse()
        {
            var @object = new User {
                Aliases = new List<string> { "My alias 1", "My alias 2", "My alias 3" },
                DisplayName = "My display name",
                IsActive = true,
                PhoneNumber = "My phone number"
            };

            var actual = this.Target.Equals(null, @object);
            Assert.IsFalse(actual);
        }


        [TestMethod]
        public void Equals_ObjectAndNull_ReturnsFalse()
        {
            var @object = new User {
                Aliases = new List<string> { "My alias 1", "My alias 2", "My alias 3" },
                DisplayName = "My display name",
                IsActive = true,
                PhoneNumber = "My phone number"
            };

            var actual = this.Target.Equals(@object, null);
            Assert.IsFalse(actual);
        }


        [TestMethod]
        public void Equals_ObjectAndACopy_ReturnsTrue()
        {
            var @object = new User {
                Aliases = new List<string> { "My alias 1", "My alias 2", "My alias 3" },
                DisplayName = "My display name",
                IsActive = true,
                PhoneNumber = "My phone number"
            };
            var other = new User {
                Aliases = new List<string>(@object.Aliases),
                DisplayName = @object.DisplayName,
                IsActive = @object.IsActive,
                PhoneNumber = @object.PhoneNumber
            };

            var actual = this.Target.Equals(@object, other);
            Assert.IsTrue(actual);
        }


        [TestMethod]
        public void Equals_DifferentPhoneNumber_ReturnsFalse()
        {
            var @object = new User {
                Aliases = new List<string> { "My alias 1", "My alias 2", "My alias 3" },
                DisplayName = "My display name",
                IsActive = true,
                PhoneNumber = "My phone number"
            };
            var other = new User {
                Aliases = new List<string>(@object.Aliases),
                DisplayName = @object.DisplayName,
                IsActive = @object.IsActive,
                PhoneNumber = @object.PhoneNumber + "Other"
            };

            var actual = this.Target.Equals(@object, other);
            Assert.IsFalse(actual);
        }

        #endregion

        #region GetHashCode

        [TestMethod]
        public void GetHashCode_ValidObject_ReturnsHashCodeOfObject()
        {
            var @object = new User {
                Aliases = new List<string> { "My alias 1", "My alias 2", "My alias 3" },
                DisplayName = "My display name",
                IsActive = true,
                PhoneNumber = "My phone number"
            };
            var expected = @object.PhoneNumber.GetHashCode();

            var actual = this.Target.GetHashCode(@object);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}