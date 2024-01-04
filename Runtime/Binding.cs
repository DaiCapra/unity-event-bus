using System;

namespace Events.Runtime
{
    public struct Binding
    {
        public Action<IEvent> action;
    }
}