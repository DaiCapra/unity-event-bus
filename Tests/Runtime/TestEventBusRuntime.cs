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