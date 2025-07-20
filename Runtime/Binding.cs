using System;

namespace Events.Runtime
{
    public struct Binding : IEquatable<Binding>
    {
        public Action<IEvent> action;
        public ulong identity;

        public bool Equals(Binding other)
        {
            return identity == other.identity;
        }

        public override bool Equals(object obj)
        {
            return obj is Binding other && Equals(other);
        }

        public override int GetHashCode()
        {
            return identity.GetHashCode();
        }
    }
}