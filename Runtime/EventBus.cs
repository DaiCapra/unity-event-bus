using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Events.Runtime
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<EventInfo>> _subscribers;

        public EventBus()
        {
            _subscribers = new Dictionary<Type, List<EventInfo>>();
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

            if (!_subscribers.ContainsKey(type))
            {
                return;
            }

            var subscribers = _subscribers[type];
            foreach (var subscriber in subscribers)
            {
                subscriber?.Target?.Invoke(e);
            }
        }

        public void Subscribe<T>(Action<T> action) where T : IEvent
        {
            var type = typeof(T);

            if (!_subscribers.ContainsKey(type))
            {
                _subscribers[type] = new List<EventInfo>();
            }

            var wrapper = new Action<IEvent>(t => { action.Invoke((T) t); });
            var data = new EventInfo(wrapper, action.Method);

            _subscribers[type].Add(data);
        }

        public void Unsubscribe<T>(Action<T> action) where T : IEvent
        {
            var type = typeof(T);

            if (!_subscribers.ContainsKey(type))
            {
                return;
            }

            var actions = _subscribers[type];
            var filter = actions
                .Where(t => t.Method.Equals(action.Method))
                .ToList();

            foreach (var f in filter)
            {
                actions.Remove(f);
            }

            if (actions.Count == 0)
            {
                _subscribers.Remove(type);
            }
        }
    }
}