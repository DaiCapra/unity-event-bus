using System;
using System.Reflection;

namespace Events.Runtime
{
    public class EventInfo
    {
        public readonly Action<IEvent> Target;
        public readonly MethodInfo Method;

        public EventInfo(Action<IEvent> target, MethodInfo method)
        {
            Target = target;
            Method = method;
        }
    }
}