﻿using System;
using DataStructures.FibonacciHeap;

namespace DataStructures.PriorityQueue
{
    public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
    {
        private readonly FibonacciHeap<TElement, TPriority> heap;

        public int Count => heap.Size();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minPriority">Minimum value of the priority - to be used for comparing.</param>
        public PriorityQueue(TPriority minPriority)
        {
            heap = new FibonacciHeap<TElement, TPriority>(minPriority);
        }

        public void Enqueue(TElement item, TPriority priority)
        {
            heap.Insert(new FibonacciHeapNode<TElement, TPriority>(item, priority));
        }

        public TElement Top()
        {
            return heap.Min().Data;
        }

        public (TElement, TPriority) Dequeue()
        {
            var node = heap.RemoveMin();
            return (node.Data, node.Key);
        }
    }
}


