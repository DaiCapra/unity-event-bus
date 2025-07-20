using System;

namespace Events.Runtime
{
    public struct EventHandle
    {
        public ulong bindingId;
        public Type type;
    }
}