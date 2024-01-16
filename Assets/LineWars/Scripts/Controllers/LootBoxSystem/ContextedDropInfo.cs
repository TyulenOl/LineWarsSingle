using System.Collections.Generic;

namespace LineWars.LootBoxes
{
    public class ContextedDropInfo
    {
        public readonly IReadOnlyList<ContextedDrop> Drops;

        public ContextedDropInfo(IReadOnlyList<ContextedDrop> drops)
        {
            Drops = drops;
        }
    }

    public class ContextedDrop
    {
        public readonly Drop Drop;
        public readonly Drop OldDrop;

        public ContextedDrop(Drop drop, Drop oldDrop = null)
        {
            Drop = drop;
            OldDrop = oldDrop;
        }
    }
}
