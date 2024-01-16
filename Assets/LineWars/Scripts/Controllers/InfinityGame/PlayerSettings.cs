using System;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    [Serializable]
    public class PlayerSettings
    {
        [SerializeField] private BasePlayer playerPrefab;
        public BasePlayer PlayerPrefab
        {
            get => playerPrefab;
            set => playerPrefab = value;
        }

        [field: SerializeField] public InitialOwnerInfo InitialOwnerInfo { get; set; }
    }
}