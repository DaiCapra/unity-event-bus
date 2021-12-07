using System;
using System.Collections.Generic;

namespace Events.Runtime
{
    public class EventBus
    {
        private readonly Dictionary<Type, EventData> _subscribers;

        public EventBus()
        {
            _subscribers = new Dictionary<Type, EventData>();
        }

        public int Count()
        {
            return _subscribers.Count;
        }

        public void Trigger<T>() where T : IEvent
        {
            Trigger(default(T));
        }

        public void Trigger(IEvent e)
        {
            var type = e.GetType();

            if (!_subscribers.TryGetValue(type, out var data))
            {
                return;
            }

            foreach (var subscriber in data.Bindings)
            {
                subscriber?.Target?.Invoke(e);
            }
        }

        public void Subscribe<T>(object obj, Action<T> action) where T : IEvent
        {
            if (obj == null)
            {
                return;
            }

            var type = typeof(T);

            if (!_subscribers.TryGetValue(type, out EventData data))
            {
                data = new EventData();
                _subscribers[type] = data;
            }


            var d = new EventBinding
            {
                Target = t => { action.Invoke((T) t); },
                Instance = obj,
            };
            data.Bindings.Add(d);
        }

        public void Unsubscribe<T>(object obj) where T : IEvent
        {
            if (obj == null)
            {
                return;
            }

            var type = typeof(T);

            if (!_subscribers.TryGetValue(type, out var data))
            {
                return;
            }

            for (var index = 0; index < data.Bindings.Count; index++)
            {
                var binding = data.Bindings[index];
                var t = binding.Instance;
                if (t != null && t.Equals(obj))
                {
                    data.Bindings.RemoveAt(index);
                    break;
                }
            }

            if (data.Bindings.Count == 0)
            {
                _subscribers.Remove(type);
            }
        }
    }
}