using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public abstract class GameRefereeCreator: ScriptableObject
    {
        protected IReadOnlyCollection<Node> Nodes;

        public void PrepareNodes(IReadOnlyCollection<Node> nodes)
        {
            Nodes = nodes;
        }

        public abstract void Initialize();
        
        /// <summary>
        /// для разных GameReferee нужны разные инициализации карт;
        /// для KingOfMountainScoreReferee нужно выбрать одну точку (желательно центральную);
        /// для SiegeGameReferee нужно настроить количество раундов до победы;
        /// </summary>
        public abstract GameReferee CreateGameReferee();
    }
}