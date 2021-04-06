using System.Collections;
using Events.Tests.Examples;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Events.Tests.Runtime
{
    public class TestEventBusRuntime
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator DestroyBehaviour()
        {
            var bus = new TestInstanceBus();
            SceneManager.LoadScene("TestA");

            yield return new WaitForSeconds(1f);
            Assert.AreEqual(0, bus.Count());

            var behaviour = Object.FindObjectOfType<TestBehaviour>();
            Assert.NotNull(behaviour);

            behaviour.Init();
            Assert.AreEqual(1, bus.Count());

            Object.Destroy(behaviour);
            yield return new WaitForSeconds(1f);

            Assert.AreEqual(0, bus.Count());
        }
    }
}