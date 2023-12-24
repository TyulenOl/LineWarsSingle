using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IAllPathGenerator<T>
    {
        public IEnumerable<string> GeneratePaths();
    }
}