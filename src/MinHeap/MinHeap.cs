using System;
using System.Collections.Generic;

namespace Chart3D.Helper.MinHeap
{
    public struct MinHeap<T>
    {
        public List<T> _stack;

        IComparer<T> _comparer;
        public int Length { get { return _stack.Count; } }
        public bool IsEmpty { get { return _stack.Count == 0; } }

        public void Clear()
        {
            _stack.Clear();
        }
        public MinHeap(IComparer<T> comparer)
        {
            _stack = new List<T>();//needed size depends on precision
            _comparer = comparer;
        }
        public void Push(T value)
        {
            _stack.Add(value);
            BubbleUp(_stack.Count - 1);
        }
        public T Pop()
        {
            T result = default;
            if (!IsEmpty)
            {
                result = _stack[0];
                DeleteRoot();
            }
            return result;
        }
        public T Peek()
        {
            T result = default;
            if (!IsEmpty)
                result = _stack[0];
            return result;
        }
        public void BubbleUp(int childIndex)
        {
            while (childIndex > 0)
            {
                int parentIndex = (childIndex - 1) / 2;
                if (_comparer.Compare(_stack[childIndex], _stack[parentIndex]) < 0)
                {
                    Swap(parentIndex, childIndex);
                    childIndex = parentIndex;
                }
                else break;
            }
        }

        public void DeleteRoot()
        {
            if (_stack.Count <= 1)
            {
                _stack.Clear();
                return;
            }
            Swap(0, _stack.Count - 1);
            _stack.RemoveAt(_stack.Count - 1);   //using _stack.RemoveAt(0) would destroy parent child relationships of heap, so RemoveAtSwapBack(0) is essential!
            BubbleDown(0);
        }
        public T Left(int index)
        {
            int leftChildIndex = index * 2 + 1;
            T result = default;
            if (_stack.Count> leftChildIndex)
                result = _stack[leftChildIndex];
            return result;
        }
        public T Right(int index)
        {
            int leftChildIndex = index * 2 + 2;
            T result = default;
            if (_stack.Count > leftChildIndex)
                result = _stack[leftChildIndex];
            return result;
        }
        public void BubbleDown(int index)
        {
            while (true)
            {
                // on each iteration exchange element with its smallest child
                int leftChildIndex = index * 2 + 1;
                int rightChildIndex = index * 2 + 2;
                int smallestItemIndex = index; // The index of the parent

                if (leftChildIndex < _stack.Count && _comparer.Compare(_stack[leftChildIndex], _stack[smallestItemIndex]) < 0)
                    smallestItemIndex = leftChildIndex;
                if (rightChildIndex < _stack.Count && _comparer.Compare(_stack[rightChildIndex], _stack[smallestItemIndex]) < 0)
                    smallestItemIndex = rightChildIndex;

                if (smallestItemIndex != index)
                {
                    Swap(smallestItemIndex, index);
                    index = smallestItemIndex;
                }
                else break;
            }
        }
        public void Swap(int pos1, int pos2)
        {
            var temp = _stack[pos1];
            _stack[pos1] = _stack[pos2];
            _stack[pos2] = temp;
        }
    }
}
