using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class MissionsMapUI : MonoBehaviour
    {
        [SerializeField] private List<MissionUI> missionUis;

        public IReadOnlyList<MissionUI> MissionUIs => missionUis;
        public int UIsCount => missionUis.Count;
    }
}