using System;
using System.Collections.Generic;
using System.Linq;

namespace Events.Runtime
{
    public class EventBus
    {
        private readonly Dictionary<Type, EventData> _subscribers;

        public EventBus()
        {
            _subscribers = new();
            History = new();
            SaveHistory = true;
            MaxHistoryCount = 100;
        }

        public int MaxHistoryCount { get; set; }

        public bool SaveHistory { get; set; }
        public List<IEvent> History { get; }


        public EventHandle Bind<T>(Action<T> action, int priority = 0) where T : IEvent
        {
            var type = typeof(T);

            if (!_subscribers.TryGetValue(type, out var eventData))
            {
                eventData = new();
                _subscribers[type] = eventData;
            }


            var binding = new Binding
            {
                id = eventData.identity++,
                action = t => { action.Invoke((T)t); },
                priority = priority
            };

            eventData.pendingSubscriptions.Add(binding);

            var handle = new EventHandle { bindingId = binding.id, type = type };
            return handle;
        }

        public int Count()
        {
            return _subscribers.Count;
        }

        public List<T> GetHistory<T>() where T : IEvent
        {
            return History
                .Where(t => t.GetType() == typeof(T))
                .Cast<T>()
                .ToList();
        }

        public bool HasHistoryAny<T>() where T : IEvent
        {
            return History.Any(t => t.GetType() == typeof(T));
        }

        public int PendingSubscriberCount()
        {
            return _subscribers.Values.Sum(t => t.pendingSubscriptions.Count);
        }

        public void Trigger<T>() where T : IEvent, new()
        {
            var t = new T();
            Trigger(t);
        }

        public void Trigger<T>(T e) where T : IEvent
        {
            AddToHistory(e);

            // Find the event data for the event type
            var type = e.GetType();
            if (!_subscribers.TryGetValue(type, out var eventData))
            {
                return;
            }

            AddPending(eventData);

            // Trigger
            foreach (var binding in eventData.bindings)
            {
                binding.action?.Invoke(e);
            }
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
                var eventData = _subscribers[type];

                eventData.Remove(eventData.bindings, handle.bindingId);
                eventData.Remove(eventData.pendingSubscriptions, handle.bindingId);

                if (eventData.bindings.Count == 0 &&
                    eventData.pendingSubscriptions.Count == 0)
                {
                    _subscribers.Remove(type);
                }
            }
        }

        private void AddBinding(List<Binding> bindings, Binding binding)
        {
            // Binary search for insertion point
            int index = bindings.BinarySearch(binding, new PriorityComparer());

            if (index < 0)
            {
                index = ~index;
            }

            bindings.Insert(index, binding);
        }

        private void AddPending(EventData eventData)
        {
            // Add waiting subscriptions and clear queue
            foreach (var binding in eventData.pendingSubscriptions)
            {
                AddBinding(eventData.bindings, binding);
            }

            eventData.pendingSubscriptions.Clear();
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
    }
}