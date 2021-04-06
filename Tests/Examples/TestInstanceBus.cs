using Events.Runtime;

namespace Events.Tests.Examples
{
    public class TestInstanceBus : EventBus
    {
        public static TestInstanceBus InstanceBus { get; set; }

        public TestInstanceBus()
        {
            InstanceBus = this;
        }
    }
}