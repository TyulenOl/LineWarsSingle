using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Interface
{
    public sealed class GameRefereeDrawer: MonoBehaviour
    {
        [SerializeField] private List<ConcreteGameRefereeDrawer> concreteGameRefereeDrawers;
        [SerializeField] private LevelInfoPanel levelInfoPanel;
        
        private Dictionary<Type, ConcreteGameRefereeDrawer> typeToDrawer;

        private void Awake()
        {
            typeToDrawer = new Dictionary<Type, ConcreteGameRefereeDrawer>();
            foreach (var drawer in concreteGameRefereeDrawers)
            {
                if (drawer == null)
                    return;
                var type = drawer.GameRefereeType;
                if (typeToDrawer.ContainsKey(type))
                    Debug.LogWarning($"Collision in {nameof(concreteGameRefereeDrawers)}!");
                
                typeToDrawer[type] = drawer;
            }
        }

        public void DrawReferee(GameReferee gameReferee)
        {
            if (gameReferee == null)
                return;
            var type = gameReferee.GetType();
            foreach (var drawer in typeToDrawer.Values)
                drawer.Hide();

            if (typeToDrawer.TryGetValue(type, out var showDrawer))
            {
                showDrawer.Show(gameReferee);
                levelInfoPanel.ReDraw(gameReferee);
            }
            else
            {
                Debug.LogWarning($"Cant draw gameReferee of type {type}");
            }
        }
    }
}