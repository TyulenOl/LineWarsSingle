using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    public abstract class MusicLogicData : ScriptableObject
    {
        public abstract MusicLogic GetMusicLogic(MusicManager manager);
    }

    public abstract class MusicLogic
    {
        protected readonly MusicManager manager;
        public MusicLogic(MusicManager manager)
        {
            this.manager = manager;
        }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}

