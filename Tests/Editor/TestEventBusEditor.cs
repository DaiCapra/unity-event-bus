using System;
using System.Collections;
using Events.Runtime;
using Events.Tests.Examples;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Events.Tests.Editor
{
    public class TestEventBusEditor
    {
        private bool _test;

        [Test]
        public void Subscribe()
        {
            var eb = new EventBus();
            eb.Subscribe<TestEvent>(TestEventBusFoo);
            Assert.AreEqual(eb.Count(), 1);
        }

        [Test]
        public void Trigger()
        {
            var eb = new EventBus();
            eb.Subscribe<TestEvent>(TestEventBusFoo);
            _test = false;
            eb.Trigger<TestEvent>();
            Assert.True(_test);
        }

        [Test]
        public void Unsubscribe()
        {
            var eb = new EventBus();
            eb.Subscribe<TestEvent>(TestEventBusFoo);

            _test = false;
            eb.Trigger<TestEvent>();
            Assert.True(_test);

            _test = false;
            eb.Unsubscribe<TestEvent>(TestEventBusFoo);
            eb.Trigger<TestEvent>();
            Assert.False(_test);
        }


        private void TestEventBusFoo(TestEvent obj)
        {
            _test = true;
        }
    }
}