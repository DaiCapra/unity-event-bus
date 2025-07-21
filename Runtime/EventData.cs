using System.Collections.Generic;

namespace Events.Runtime
{
    public class EventData
    {
        public ulong identity;
        public readonly Dictionary<ulong, Binding> bindings = new();
        public readonly List<Binding> pendingSubscriptions = new();
    }
}