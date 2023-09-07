using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(fileName = "New Random Music Logic", menuName = "Audio/Music Logic/Silent")]
    public class SilentMusicLogicData : MusicLogicData
    {
        public override MusicLogic GetMusicLogic(MusicManager manager)
        {
            return new SilentMusicLogic(manager);
        }
    }

    public class SilentMusicLogic : MusicLogic
    {
        public SilentMusicLogic(MusicManager manager) : base(manager)
        {
        }
    }
}

