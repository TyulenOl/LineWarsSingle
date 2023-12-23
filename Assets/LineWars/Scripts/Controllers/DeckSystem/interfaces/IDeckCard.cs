using UnityEngine;

namespace LineWars.Model
{ 
    public interface IDeckCard
    {
        public string Name { get; } 
        public string Description { get; }
        public Unit Unit { get; }   
    }
}
