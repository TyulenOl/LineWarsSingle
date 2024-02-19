using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(fileName = "New Random Music Logic", menuName = "Audio/Plugin/Music Logic/Silent")]
    public class PluginSilentMusicLogicData : PluginMusicLogicData
    {
        public override PluginMusicLogic GetMusicLogic(PluginMusicManager manager)
        {
            return new PluginSilentMusicLogic(manager);
        }
    }

    public class PluginSilentMusicLogic : PluginMusicLogic
    {
        public PluginSilentMusicLogic(PluginMusicManager manager) : base(manager)
        {
        }
    }
}

