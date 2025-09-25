using System;
using System.Collections.Generic;
using Events.Runtime;
using Events.Tests.Examples;
using NUnit.Framework;

namespace Events.Tests
{
    public class TestEventBusEditor
    {
        private bool _testA;
        private bool _testB;

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

            _testA = false;

            _eventBus.Trigger<TestEvent>();
            Assert.True(_testA);
        }

        [Test]
        public void Priority()
        {
            var executionOrder = new List<int>();
    
            _eventBus.Bind<TestEvent>(_ => { executionOrder.Add(1); }, priority: 1);
            _eventBus.Bind<TestEvent>(_ => { executionOrder.Add(2); }, priority: 2);
            _eventBus.Bind<TestEvent>(_ => { executionOrder.Add(0); }, priority: 0);
    
            _eventBus.Trigger(new TestEvent());
            Assert.That(executionOrder, Is.EqualTo(new List<int> { 2, 1, 0 }));
        }

        [Test]
        public void Unsubscribe()
        {
            var handle = _eventBus.Bind<TestEvent>(TestEventBusFoo);

            _testA = false;

            _eventBus.Trigger<TestEvent>();
            Assert.True(_testA);

            _testA = false;

            _eventBus.Unbind(handle);
            _eventBus.Trigger<TestEvent>();
            Assert.False(_testA);
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
            _testA = true;
        }
    }
}