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
            eb.Bind<TestEvent>(TestEventBusFoo);
            Assert.AreEqual(eb.Count(), 1);
        }

        [Test]
        public void Trigger()
        {
            var eb = new EventBus();
            eb.Bind<TestEvent>(TestEventBusFoo);
            _test = false;
            eb.Trigger<TestEvent>();
            Assert.True(_test);
        }

        [Test]
        public void Unsubscribe()
        {
            var eb = new EventBus();
            var handle = eb.Bind<TestEvent>(TestEventBusFoo);

            _test = false;
            eb.Trigger<TestEvent>();
            Assert.True(_test);

            _test = false;
            eb.Unbind(handle);
            eb.Trigger<TestEvent>();
            Assert.False(_test);
        }

        private void TestEventBusFoo(TestEvent obj)
        {
            _test = true;
        }
    }
}