using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

using Altidude.Contracts;
using Altidude.Domain;
using Altidude.Infrastructure;
using KellermanSoftware.CompareNetObjects;

namespace Domain.Test
{
    [TestClass]
    public class BDDTestBase
    {
        private InMemoryDomainRespository _domainRepository;
        private Dictionary<Guid, IEnumerable<IEvent>> _preConditions = new Dictionary<Guid, IEnumerable<IEvent>>();

        public IDateTimeProvider DateTimeProvider { get; set; }
        public IUserService UserService { get; set; }
        public IUserLevelService UserLevelService { get; set; }
        public IPlaceRepository PlaceRepository { get; set; }

        private DomainEntry BuildApplication()
        {
            _domainRepository = new InMemoryDomainRespository();
            _domainRepository.AddEvents(_preConditions);

            return new DomainEntry(_domainRepository, null, DateTimeProvider, UserService, UserLevelService, PlaceRepository as IPlaceFinder, null);
        }

        [TestInitialize]
        public void Initialize()
        {
            UserLevelService = new InMemoryUserLevelService();
            DateTimeProvider = new ManualDateTimeProvider();
            PlaceRepository = new InMemoryPlaceRepository();
        }

        [TestCleanup]
        public void TearDown()
        {
            _preConditions = new Dictionary<Guid, IEnumerable<IEvent>>();
        }

        protected void When(ICommand command)
        {
            var application = BuildApplication();
            application.ExecuteCommand(command);
        }

        protected void Then(params IEvent[] expectedEvents)
        {
            var latestEvents = _domainRepository.GetLatestEvents().ToList();
            var expectedEventsList = expectedEvents.ToList();
            Assert.AreEqual(expectedEventsList.Count, latestEvents.Count);

            var compareLogic = new CompareLogic(new ComparisonConfig { MaxDifferences = 10 });

            for (int i = 0; i < latestEvents.Count; i++)
            {
                var compareResult = compareLogic.Compare(expectedEvents[i], latestEvents[i]);

                Assert.IsTrue(compareResult.AreEqual, compareResult.DifferencesString);
            }
        }

        protected void WhenThrows<TException>(ICommand command) where TException : Exception
        {
            Action act = () => When(command);

            act.ShouldThrow<TException>();
        }

        protected void Given(params IEvent[] existingEvents)
        {
            _preConditions = existingEvents
                .GroupBy(y => y.Id)
                .ToDictionary(y => y.Key, y => y.AsEnumerable());
        }
    }
}
