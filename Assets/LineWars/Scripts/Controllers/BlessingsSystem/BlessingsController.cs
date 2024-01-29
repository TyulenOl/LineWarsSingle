using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class BlessingsController : MonoBehaviour, IBlessingSelector
    {
        [SerializeField] private int maxBlessingsIdsCount;
        [SerializeField] private int totalBlessingsCount;

        private IBlessingSelector globalBlessingSelector;
        private IBlessingsPull globalBlessingPull;
        private IStorage<BlessingId, BaseBlessing> blessingStorage;

        public int MaxBlessingsIdsCount => maxBlessingsIdsCount;
        public int TotalBlessingsCount => totalBlessingsCount;

        public IBlessingSelector SelectedBlessings => this;
        public IBlessingsPull BlissingPullForGame => new LimitingBlessingPool(globalBlessingPull, SelectedBlessings, totalBlessingsCount);


        public void Initialize(
            IBlessingSelector globalBlessingSelector,
            IBlessingsPull globalBlessingPull,
            IStorage<BlessingId, BaseBlessing> blessingStorage)
        {
            this.globalBlessingSelector = globalBlessingSelector;
            this.globalBlessingPull = globalBlessingPull;
            this.blessingStorage = blessingStorage;
      
            AssignInnerBlessingSelector();
        }

        public int GetBlessingsCount(BlessingId blessingId)
        {
            return Math.Min(globalBlessingPull[blessingId], TotalBlessingsCount);
        }

        private void AssignInnerBlessingSelector()
        {
            globalBlessingSelector.Count = MaxBlessingsIdsCount;
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return SelectedBlessings.GetEnumerator();
        }

        IEnumerator<BlessingId> IEnumerable<BlessingId>.GetEnumerator()
        {
            return globalBlessingSelector.GetEnumerator();
        }
        int IReadOnlyCollection<BlessingId>.Count => globalBlessingSelector.Count;

        int IBlessingSelector.Count
        {
            get => globalBlessingSelector.Count;
            set => throw new InvalidOperationException();
        }


        BlessingId IBlessingSelector.this[int index]
        {
            get => globalBlessingSelector[index];
            set
            {
                if (SelectedBlessings.CanSetValue(index, value))
                    throw new InvalidOperationException();
                globalBlessingSelector[index] = value;
            }
        }

        bool IBlessingSelector.CanSetValue(int index, BlessingId blessingId)
        {
            return globalBlessingSelector.CanSetValue(index, blessingId)
                   && !globalBlessingSelector.Contains(blessingId);
        }
    }
}