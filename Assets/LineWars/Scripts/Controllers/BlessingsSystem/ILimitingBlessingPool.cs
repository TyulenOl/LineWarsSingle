using System;

namespace LineWars.Controllers
{
    public interface ILimitingBlessingPool: IBlessingsPull
    {
        int CurrentTotalCount { get; }
        int TotalCount { get; }
        event Action<int, int> CurrentTotalCountChanged;
    }
}