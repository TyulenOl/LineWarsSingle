using System;
using LineWars.Extensions.Attributes;
using UnityEngine;

namespace LineWars.Controllers
{
    public class SingleGameManager: MonoBehaviour
    {
        public SingleGameManager Instance { get; private set; }
        [SerializeField, ReadOnlyInspector] private string mapToLoad;
        
        private void Awake()
        {
            Instance = this;
            

        }

        private void Start()
        {
            mapToLoad = "TestMap";
            
            if (LevelsManager.ExistenceMap(mapToLoad))
                LevelsManager.LoadMap(mapToLoad);
        }

        public void ToMainMenu()
        {
            SceneTransition.LoadScene(SceneName.MainMenu);
        }
    }
}