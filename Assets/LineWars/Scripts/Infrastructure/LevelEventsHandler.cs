using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LineWars.Infrastructure
{
    public abstract class LevelEventsHandler: MonoBehaviour
    {
        [SerializeField] private SingleGameRoot singleGameRoot;
        private bool started;
        public SceneName Scene => (SceneName)SceneManager.GetActiveScene().buildIndex;
        private void Start()
        {
            if (singleGameRoot == null)
                return;

            singleGameRoot.Won += OnWinLevel;
            singleGameRoot.Lost += OnLoseLevel;
            SceneTransition.StartLoad += SceneTransitionOnStartLoad;
            
            if (singleGameRoot.GameStarted)
            {
                OnLevelStart();
            }
            else
            {
                singleGameRoot.OnGameStart += OnLevelStart;
            }
        }

        private void OnDestroy()
        {
            if (singleGameRoot != null)
            {
                singleGameRoot.Won -= OnWinLevel;
                singleGameRoot.Lost -= OnLoseLevel;
                singleGameRoot.OnGameStart -= OnLevelStart;
            }
            
            SceneTransition.StartLoad -= SceneTransitionOnStartLoad;
        }
        
        private void SceneTransitionOnStartLoad()
        {
            OnExitLevel();
        }

        private void OnLevelStart()
        {
            started = true;
            LevelStarted();
        }

        private void OnWinLevel()
        {
            if (started)
                LevelFinished(LevelFinishStatus.Win);
            started = false;
        }
        
        private void OnLoseLevel()
        {
            if (started)
                LevelFinished(LevelFinishStatus.Lose);
            started = false;
        }
        
        private void OnExitLevel()
        {
            if (started)
                LevelFinished(LevelFinishStatus.Exit);
            started = false;
        }

        protected abstract void LevelStarted();
        protected abstract void LevelFinished(LevelFinishStatus levelFinishStatus);
    }
}