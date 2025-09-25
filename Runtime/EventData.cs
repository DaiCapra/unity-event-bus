using System.Collections.Generic;

namespace Events.Runtime
{
    public class EventData
    {
        public ulong identity;
        public readonly List<Binding> bindings = new();
        public readonly List<Binding> pendingSubscriptions = new();

        public bool Remove(List<Binding> list, ulong id)
        {
            var index = list.FindIndex(b => b.id == id);
            if (index != -1)
            {
                list.RemoveAt(index);
                return true;
            }

            return false;
        }
    }
}