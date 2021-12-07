using Events.Runtime;
using Events.Tests.Examples;
using NUnit.Framework;

namespace Events.Tests.Editor
{
    public class TestEventBusEditor
    {
        private bool _test;

        [Test]
        public void Subscribe()
        {
            var eb = new EventBus();
            eb.Subscribe<TestEvent>(this, TestEventBusFoo);
            Assert.AreEqual(eb.Count(), 1);
        }

        [Test]
        public void Trigger()
        {
            var eb = new EventBus();
            eb.Subscribe<TestEvent>(this, TestEventBusFoo);
            _test = false;
            eb.Trigger<TestEvent>();
            Assert.True(_test);
        }

        [Test]
        public void Unsubscribe()
        {
            var eb = new EventBus();
            eb.Subscribe<TestEvent>(this, TestEventBusFoo);

            _test = false;
            eb.Trigger<TestEvent>();
            Assert.True(_test);

            _test = false;
            eb.Unsubscribe<TestEvent>(this);
            eb.Trigger<TestEvent>();
            Assert.False(_test);
        }

        [Test]
        public void MultipleInstances()
        {
            var eb = new EventBus();
            var alice = new object();
            var bob = new object();
            var charlie = new object();
            var counter = 0;
            
            eb.Subscribe<TestEvent>(alice, _ => counter++);
            eb.Subscribe<TestEvent>(bob, _ => counter++);
            eb.Subscribe<TestEvent>(charlie, _ => counter++);
                
            eb.Trigger<TestEvent>();
            Assert.That(counter, Is.EqualTo(3));
            
            eb.Unsubscribe<TestEvent>(alice);
            eb.Trigger<TestEvent>();
            Assert.That(counter, Is.EqualTo(5));
        }

        private void TestEventBusFoo(TestEvent obj)
        {
            _test = true;
        }
    }
}