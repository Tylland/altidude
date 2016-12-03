using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Altidude.Contracts.Commands;
using Altidude.Contracts.Events;
using Altidude.Domain;
using Altidude.Infrastructure;
using Altidude.Contracts.Types;

namespace Domain.Test
{
    [TestClass]
    public class UserBehavior : BDDTestBase
    {
        ManualDateTimeProvider _dateTime;

        [TestInitialize]
        public void Init()
        {
            _dateTime = new ManualDateTimeProvider();
            _dateTime.Now = DateTime.Now;

            DateTimeProvider = _dateTime;
        }

        [TestMethod]
        public void WhenCreateUser_ThenUserCreated()
        {
            var userId = Guid.NewGuid();

            _dateTime.Now = DateTime.Now;

            When(new CreateUser(userId, "testUser", "qwerty@abc.xyx", "first", "last"));
            Then(new UserCreated(userId, "testUser", "qwerty@abc.xyx", "first", "last", _dateTime.Now));
        }
        [TestMethod]
        public void WhenRegisterUserExperience_ThenUserGainedExperience()
        {
            var userId = Guid.NewGuid();

            Given(new UserCreated(userId, "testUser", "qwerty@abc.xyx", "first", "last", _dateTime.Now));
            When(new RegisterUserExperience(userId, 10));
            Then(new UserGainedExperience(userId, 10, 10), new UserGainedLevel(userId, new UserLevel(1, 0, 100)));
        }

    }
}
