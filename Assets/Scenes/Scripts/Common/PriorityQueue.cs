using System;
using System.Collections.Generic;

namespace LegoBattaleRoyal.Common
{
    public class PriorityQueue<T, P>
    {
        private readonly List<(T item, P priority)> _items = new();
        private readonly IComparer<P> _priorityComparer;

        public int Count => _items.Count;

        public PriorityQueue(IComparer<P> priorityComparer = null)
        {
            _priorityComparer = priorityComparer ?? Comparer<P>.Default;
        }

        public void Enqueue(T item, P priority)
        {
            _items.Add((item, priority));
            var childIndex = _items.Count - 1;

            while (childIndex > 0)
            {
                var parentIndex = (childIndex - 1) / 2;

                if (_priorityComparer.Compare(_items[childIndex].priority, _items[parentIndex].priority) >= 0)
                    break;

                Swap(childIndex, parentIndex);
                childIndex = parentIndex;
            }
        }

        public T Dequeue()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Queue is empty");

            var (item, _) = _items[0];
            var lastIndex = _items.Count - 1;
            _items[0] = _items[lastIndex];
            _items.RemoveAt(lastIndex);

            lastIndex--;

            var parentIndex = 0;

            while (true)
            {
                var leftChildIndex = (parentIndex * 2) + 1;
                if (leftChildIndex > lastIndex)
                    break;

                var rightChildIndex = leftChildIndex + 1;
                if (rightChildIndex <= lastIndex
                    && _priorityComparer.Compare(_items[rightChildIndex].priority, _items[leftChildIndex].priority) < 0)
                    leftChildIndex = rightChildIndex;

                if (_priorityComparer.Compare(_items[parentIndex].priority, _items[leftChildIndex].priority) <= 0)
                    break;

                Swap(parentIndex, leftChildIndex);
                parentIndex = leftChildIndex;
            }

            return item;
        }

        private void Swap(int indexA, int indexB)
        {
            (_items[indexB], _items[indexA]) = (_items[indexA], _items[indexB]);
        }
    }
}