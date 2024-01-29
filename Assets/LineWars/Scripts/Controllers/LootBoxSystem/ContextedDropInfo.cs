using System.Collections.Generic;

namespace LineWars.Model
{
    public class ContextedDropInfo
    {
        public readonly IReadOnlyList<ContextedDrop> Drops;

        public ContextedDropInfo(IReadOnlyList<ContextedDrop> drops)
        {
            Drops = drops;
        }
    }
}
