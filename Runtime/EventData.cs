using System.Collections.Generic;

namespace Events.Runtime
{
    public class EventData
    {
        public List<EventBinding> Bindings;

        public EventData()
        {
            Bindings = new List<EventBinding>();
        }
    }
}