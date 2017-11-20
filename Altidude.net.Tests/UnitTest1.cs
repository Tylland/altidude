using System;
using Altidude.Application;
using Altidude.Contracts;
using Altidude.Contracts.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Altidude.Infrastructure;
using Altidude.Contracts.Events;

namespace Altidude.net.Tests
{
    [TestClass]
    public class EventHandlerContainerTest
    {
        [TestMethod]
        public void OrmLiteProfileViewIsIHandleEvent_ProfileCreated_()
        {
            var view = new OrmLiteProfileView(null);

            Assert.IsTrue(view is IHandleEvent<ProfileCreated>);
            Assert.IsNotNull(view as IHandleEvent<ProfileCreated>);
        }

        [TestMethod]
        public void OrmLiteProfileViewShouldBeResolved()
        {
            var view = new OrmLiteProfileView(null);

            var eventHandlers = new EventHandlerContainer();

            eventHandlers.Add(view);

            var handlers = eventHandlers.ResolveAll<IHandleEvent<ProfileCreated>>();

            Assert.AreEqual(1, handlers.Length);
        }
    }
}
