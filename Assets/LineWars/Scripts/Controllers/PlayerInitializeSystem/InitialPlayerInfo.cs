using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    [Serializable]
    public class InitialPlayerInfo
    {
        [field: SerializeField] public BasePlayer Player { get; set; }
        public Nation Nation => Player ? Player.Nation : null;

        [field: SerializeField] public Node MainSpawn { get; set; }
        [field: SerializeField] public List<Node> InitialSpawns { get; set; }
        [field: SerializeField] public List<Node> InitialNodes { get; set; }
        
        
        public void Validate()
        { 
            MainSpawn = MainSpawn ? MainSpawn : InitialSpawns.FirstOrDefault();
        }
    }
}