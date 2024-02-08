using System;
using System.Collections.Generic;
using LineWars.Model;

namespace LineWars.Controllers
{
    public interface IBlessingSelector: IReadOnlyCollection<BlessingId>
    {
        public event Action<BlessingId, int> SelectedBlessingIdChanged;
        public event Action<int> TotalSelectionCountChanged; 
        
        public BlessingId this[int index] { get; set; }
        public new int Count { get; set; }
        public bool CanSetValue(int index, BlessingId blessingId);
    }
}