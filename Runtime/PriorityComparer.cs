using System.Collections.Generic;

namespace Events.Runtime
{
    public class PriorityComparer : IComparer<Binding>
    {
        public int Compare(Binding x, Binding y)
        {
            var priorityComparison = y.priority.CompareTo(x.priority);
            return priorityComparison != 0
                ? priorityComparison
                : x.id.CompareTo(y.id);
        }
    }
}