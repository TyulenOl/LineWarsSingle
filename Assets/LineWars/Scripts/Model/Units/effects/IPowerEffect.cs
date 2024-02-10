using System;

namespace LineWars.Model
{
    public interface IPowerEffect
    {
        public int Power { get; }
        public event Action<IPowerEffect, int, int> PowerChanged;
    }
}
