using System;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace LineWars.Model
{
    public interface IAlive
    {
        public int CurrentHp { get; set; }
    }
}