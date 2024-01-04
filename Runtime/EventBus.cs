using System;
using System.Collections.Generic;
using System.Linq;

namespace Events.Runtime
{
    public class EventBus
    {
        private const int MaxHistoryCount = 100;
        private readonly Dictionary<Type, EventData> _subscribers;

        public EventBus()
        {
            _subscribers = new();
            History = new();
            SaveHistory = true;
        }

        public bool SaveHistory { get; set; }
        public List<IEvent> History { get; }

        public EventHandle Bind<T>(Action<T> action) where T : IEvent
        {
            var type = typeof(T);
            if (!_subscribers.TryGetValue(type, out var data))
            {
                data = new EventData();
                _subscribers[type] = data;
            }

            var binding = new Binding
            {
                action = t => { action.Invoke((T)t); }
            };

            var id = data.identity;
            data.identity++;

            data.bindings.Add(id, binding);
            return new() { id = id, type = type };
        }

        public void Unbind(EventHandle handle)
        {
            var type = handle.type;
            if (type == null)
            {
                return;
            }

            if (_subscribers.ContainsKey(type))
            {
                var data = _subscribers[type];
                data.bindings.Remove(handle.id);

                if (data.bindings.Count == 0)
                {
                    _subscribers.Remove(type);
                }
            }
        }

        public void Trigger<T>() where T : IEvent, new()
        {
            var t = new T();
            Trigger(t);
        }

        public bool HasHistoryAny<T>() where T : IEvent
        {
            return History.Any(t => t.GetType() == typeof(T));
        }

        public List<T> GetHistory<T>() where T : IEvent
        {
            return History
                .Where(t => t.GetType() == typeof(T))
                .Cast<T>()
                .ToList();
        } 
        
        public void Trigger<T>(T e) where T : IEvent
        {
            AddToHistory(e);

            var type = e.GetType();
            if (!_subscribers.ContainsKey(type))
            {
                return;
            }

            var data = _subscribers[type];
            foreach (var kv in data.bindings)
            {
                var binding = kv.Value;
                binding.action?.Invoke(e);
            }
        }

        private void AddToHistory<T>(T e) where T : IEvent
        {
            if (!SaveHistory)
            {
                return;
            }

            History.Add(e);
            if (History.Count >= MaxHistoryCount)
            {
                History.RemoveAt(0);
            }
        }

        public int Count()
        {
            return _subscribers.Count;
        }
    }
}