using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars
{
    public class CompanyMenu : UIStackElement
    {
        [SerializeField] private MissionInfoUI missionInfoUI;
        [SerializeField] private List<MissionUI> missionUis;
        private bool initialized;

        public IEnumerable<MissionInfo> MissionInfos => missionUis.Select(x => x.MissionInfo);
        public void Initialize()
        {
            if (initialized)
            {
                Debug.LogError($"{GetType().Name} is initialized!");
                return;
            }
            initialized = true;

            foreach (var missionUi in missionUis)
            {
                missionUi.Initialize(missionInfoUI);
            }
        }

        public override void OnOpen()
        {
            base.OnOpen();
            missionInfoUI.gameObject.SetActive(false);
        }
    }
}