using System.Collections.Generic;

namespace LineWars.Model
{
    public class DropInfo
    {
        public readonly IReadOnlyList<Drop> Drops;

        public DropInfo(IReadOnlyList<Drop> drops)
        {
            Drops = drops;
        }
    }
}
