using System;
using System.Reflection;

namespace Events.Runtime
{
    public class EventBinding
    {
        public Action<IEvent> Target;
        public object Instance;
    }
}