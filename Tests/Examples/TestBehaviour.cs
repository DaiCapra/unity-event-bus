using System;
using UnityEngine;

namespace Events.Tests.Examples
{
    public class TestBehaviour : MonoBehaviour
    {
        public void Init()
        {
            TestInstanceBus.InstanceBus?.Subscribe<TestEvent>(Foo);
        }

        private void OnDestroy()
        {
            TestInstanceBus.InstanceBus?.Unsubscribe<TestEvent>(Foo);
        }

        private void Foo(TestEvent obj)
        {
        }
    }
}