using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Logic;
using Models;
using NUnit.Framework;

namespace Tests
{
    public class UserLogicIntegrationTests
    {
        private UserLogic _logic;

        [SetUp]
        public void Setup()
        {
            _logic = new UserLogic(new UserContext());
        }

        [Test]
        public void GetUserByEmailExistingEmail()
        {
            var email = "ensar123@hotmail.com";
            var expectedUserId = 3006;

            var user = _logic.GetUserByEmail(email);

            Assert.AreEqual(expectedUserId, user.Id);
        }

        [Test]
        public void GetUserByEmailNonExistingEmail()
        {
            var email = "fakeEmail@@@hotmail.com";
            var expectedUserId = 0;

            var user = _logic.GetUserByEmail(email);

            Assert.AreEqual(expectedUserId, user.Id);
        }

        [Test]
        public void GetUserByEmailEmptyEmail()
        {
            var email = "";
            var expectedUserId = 0;

            var user = _logic.GetUserByEmail(email);

            Assert.AreEqual(expectedUserId, user.Id);
        }

        [Test]
        public void IsEmailInUseExistingEmail()
        {
            var existingEmail = "ensar123@hotmail.com";
            var outcome = _logic.IsEmailInUse(existingEmail);

            Assert.AreEqual(true, outcome);
        }

        [Test]
        public void IsEmailInUseNonExistingEmail()
        {
            var existingEmail = "fakeEmail@@@hotmail.com";
            var outcome = _logic.IsEmailInUse(existingEmail);

            Assert.AreEqual(false, outcome);
        }

        [Test]
        public void GetUserRolesExistingUser()
        {
            var userToGetRolesFrom = new User
            {
                Email = "ensar123@hotmail.com"
            };

            var expectedRoles = new List<string>
            {
                "AccountHolder"
            };

            var userRoles = _logic.GetUserRoles(userToGetRolesFrom);

            Assert.AreEqual(expectedRoles, userRoles);
        }

        [Test]
        public void GetUserRolesNonExistingUser()
        {
            var userToGetRolesFrom = new User
            {
                Email = "fakeEmail@@@hotmail.com"
            };

            var expectedRoles = new List<string>();

            var userRoles = _logic.GetUserRoles(userToGetRolesFrom);

            Assert.AreEqual(expectedRoles, userRoles);
        }
    }
}
