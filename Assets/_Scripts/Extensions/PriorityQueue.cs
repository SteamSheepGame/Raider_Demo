using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Core
{
    public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
    {
        private readonly List<(TElement element, TPriority priority)> _elements = new List<(TElement, TPriority)>();
        private readonly Dictionary<TElement, int> _indexMap = new Dictionary<TElement, int>();
        
        public int Count => _elements.Count;
        
        public void Enqueue(TElement element, TPriority priority)
        {
            _elements.Add((element, priority));
            _indexMap[element] = _elements.Count - 1;
            HeapUp(_elements.Count - 1);
        }
        
        public TElement Dequeue()
        {
            if (_elements.Count == 0)
                throw new InvalidOperationException("The priority queue is empty.");
            var result = _elements[0].element;
            RemoveAt(0);
            return result;
        }
        
        public void Remove(TElement element)
        {
            if (_indexMap.TryGetValue(element, out int index))
                RemoveAt(index);
        }
        
        public bool TryPeek(out TElement element, out TPriority priority)
        {
            if (_elements.Count == 0)
            {
                element = default;
                priority = default;
                return false;
            }
            element = _elements[0].element;
            priority = _elements[0].priority;
            return true;
        }
        
        public void UpdatePriority(TElement element, TPriority newPriority)
        {
            if (!_indexMap.TryGetValue(element, out int index))
                return;
            var oldPriority = _elements[index].priority;
            _elements[index] = (element, newPriority);
            if (newPriority.CompareTo(oldPriority) < 0)
                HeapUp(index);
            else
                HeapDown(index);
        }
        
        public IEnumerable<(TElement element, TPriority priority)> GetAll()
        {
            return _elements;
        }
        
        private void RemoveAt(int index)
        {
            if (index < 0 || index >= _elements.Count) return;
            _indexMap.Remove(_elements[index].element);
            if (index == _elements.Count - 1)
            {
                _elements.RemoveAt(index);
                return;
            }
            _elements[index] = _elements[_elements.Count - 1];
            _indexMap[_elements[index].element] = index;
            _elements.RemoveAt(_elements.Count - 1);
            if (index < _elements.Count) HeapDown(index);
        }
        
        private void HeapUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_elements[index].priority.CompareTo(_elements[parent].priority) >= 0) break;
                Swap(index, parent);
                index = parent;
            }
        }

        private void HeapDown(int index)
        {
            int minChild;
            while ((minChild = 2 * index + 1) < _elements.Count)
            {
                if (minChild + 1 < _elements.Count && _elements[minChild + 1].priority.CompareTo(_elements[minChild].priority) < 0)
                    minChild++;
                if (_elements[index].priority.CompareTo(_elements[minChild].priority) <= 0) break;
                Swap(index, minChild);
                index = minChild;
            }
        }
        
        private void Swap(int i, int j)
        {
            (_elements[i], _elements[j]) = (_elements[j], _elements[i]);
            _indexMap[_elements[i].element] = i;
            _indexMap[_elements[j].element] = j;
        }
    }
}
