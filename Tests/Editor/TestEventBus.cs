using System;
using Events.Runtime;
using NUnit.Framework;

namespace Events.Tests.Editor
{
    public struct EventTest : IEvent
    {
    }

    public class TestEventBus
    {
        private bool _test;

        [Test]
        public void Subscribe()
        {
            var eb = new EventBus();
            eb.Subscribe<EventTest>(TestEventBusFoo);
            Assert.AreEqual(eb.NumberOfUniqueEvents, 1);
        }

        [Test]
        public void Trigger()
        {
            var eb = new EventBus();
            eb.Subscribe<EventTest>(TestEventBusFoo);
            _test = false;
            eb.Trigger<EventTest>();
            Assert.True(_test);
        }

        [Test]
        public void Unsubscribe()
        {
            var eb = new EventBus();
            eb.Subscribe<EventTest>(TestEventBusFoo);
            
            _test = false;
            eb.Trigger<EventTest>();
            Assert.True(_test);
            
            _test = false;
            eb.Unsubscribe<EventTest>(TestEventBusFoo);
            eb.Trigger<EventTest>();
            Assert.False(_test);
        }


        private void TestEventBusFoo(EventTest obj)
        {
            _test = true;
        }
    }
}