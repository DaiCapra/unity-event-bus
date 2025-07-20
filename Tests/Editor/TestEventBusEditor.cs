using Events.Runtime;
using Events.Tests.Examples;
using NUnit.Framework;

namespace Events.Tests
{
    public class TestEventBusEditor
    {
        private bool _test;

        private EventBus _eventBus;

        [Test]
        public void CollectionModifiedException()
        {
            _eventBus.Bind<TestEvent>(_ =>
            {
                // This should not throw a CollectionModifiedException
                _eventBus.Bind<TestEvent>(TestEventBusFoo);
            });

            _eventBus.Trigger<TestEvent>();
        }

        [SetUp]
        public void Setup()
        {
            _eventBus = new();
        }

        [Test]
        public void Subscribe()
        {
            _eventBus.Bind<TestEvent>(TestEventBusFoo);
            Assert.AreEqual(_eventBus.Count(), 1);
            Assert.That(_eventBus.PendingSubscriberCount(), Is.EqualTo(1));
        }

        [Test]
        public void Trigger()
        {
            _eventBus.Bind<TestEvent>(TestEventBusFoo);

            _test = false;

            _eventBus.Trigger<TestEvent>();
            Assert.True(_test);
        }

        [Test]
        public void Unsubscribe()
        {
            var handle = _eventBus.Bind<TestEvent>(TestEventBusFoo);

            _test = false;

            _eventBus.Trigger<TestEvent>();
            Assert.True(_test);

            _test = false;

            _eventBus.Unbind(handle);
            _eventBus.Trigger<TestEvent>();
            Assert.False(_test);
        }

        [Test]
        public void UnsubscribePending()
        {
            var handle = _eventBus.Bind<TestEvent>(TestEventBusFoo);
            Assert.That(_eventBus.PendingSubscriberCount(), Is.EqualTo(1));

            _eventBus.Unbind(handle);
            Assert.That(_eventBus.PendingSubscriberCount(), Is.EqualTo(0));
        }

        private void TestEventBusFoo(TestEvent obj)
        {
            _test = true;
        }
    }
}