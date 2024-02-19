using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    public abstract class PluginMusicLogicData : ScriptableObject
    {
        public abstract PluginMusicLogic GetMusicLogic(PluginMusicManager manager);
    }

    public abstract class PluginMusicLogic
    {
        protected readonly PluginMusicManager manager;
        public PluginMusicLogic(PluginMusicManager manager)
        {
            this.manager = manager;
        }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}

