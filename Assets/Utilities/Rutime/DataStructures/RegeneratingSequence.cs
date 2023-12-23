using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public class RegeneratingSequence: IEnumerable<int>
    {
        private readonly int start;
        private readonly IEnumerator<int> occupiedPositionsEnumerator;

        private int next;
        private int nextOccupied;

        public RegeneratingSequence(int start, IEnumerable<int> occupiedPositions)
        {
            this.start = start;
            next = start;
            occupiedPositionsEnumerator = occupiedPositions
                .Distinct()
                .OrderBy(e => e)
                .GetEnumerator();
            
            FindNextOccupiedPosition();
        }

        private void FindNextOccupiedPosition()
        {
            nextOccupied = occupiedPositionsEnumerator.MoveNext()
                ? occupiedPositionsEnumerator.Current
                : start - 1;
        }

        public int Peek()
        {
            while (true)
            {
                if (next != nextOccupied)
                {
                    return next;
                }

                FindNextOccupiedPosition();
                next++;
            }
        }

        public int Pop()
        {
            Peek();
            return next++;
        }

        public IEnumerator<int> GetEnumerator()
        {
            while (true)
            {
                yield return Pop();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}