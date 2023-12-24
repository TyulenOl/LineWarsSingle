using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class SettingsProvider: ScriptableObject, IProvider<Settings> 
    {
        public abstract void Save(Settings value, int id);
        public abstract Settings Load(int id);
        public abstract IEnumerable<Settings> LoadAll();
    }
}