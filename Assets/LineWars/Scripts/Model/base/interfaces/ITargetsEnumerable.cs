using System.Collections.Generic;

namespace LineWars.Model
{
    public interface ITargetsEnumerable
    {
        public IEnumerable<ITarget> Targets { get; }
    }
}