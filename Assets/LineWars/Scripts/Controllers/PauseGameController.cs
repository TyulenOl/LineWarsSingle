using UnityEngine;

namespace LineWars.Controllers
{
    public class PauseGameController: MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float volumeValue = 0.6f;
        
        private MusicManager musicManager;
        
        public bool IsPause { get; private set; }
        private float previousValue;
        
        public void Initialize(
            MusicManager musicManager)
        {
            this.musicManager = musicManager;
        }


        public void Pause()
        {
            if (IsPause)
                return;
            IsPause = true;
            previousValue = musicManager.Source.volume;
            musicManager.Source.volume *= volumeValue;
        }
        
        public void Resume()
        {
            if (!IsPause)
                return;
            IsPause = false;
            musicManager.Source.volume = previousValue;
        }
    }
}