using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(menuName = "InfinityGame/GameRefereeCreators/MapCapture", order = 59)]
    public class MapCaptureCreator: GameRefereeCreator
    {
        [SerializeField] private int scoreForWin;
        
        public override void Initialize()
        {
            foreach (var node in Nodes)
            {
                node.gameObject.AddComponent<NodeScore>().Score = 1;
            }
        }

        public override GameReferee CreateGameReferee()
        {
            var gameReferee = new GameObject().AddComponent<MapCaptureScoreReferee>();
            gameReferee.ScoreForWin = scoreForWin;
            return gameReferee;
        }
    }
}