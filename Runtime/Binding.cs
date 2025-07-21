using System;

namespace Events.Runtime
{
    public struct Binding : IEquatable<Binding>
    {
        public Action<IEvent> action;
        public ulong id;

        public bool Equals(Binding other)
        {
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            return obj is Binding other && Equals(other);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}