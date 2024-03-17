using System.Collections;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(fileName = "new Single Repeat Music Logic", menuName = "Audio/Music Logic/Single Repeat")]
    public class SingleRepeatMusicLogicData : MusicLogicData
    {
        [SerializeField] private MusicData musicData; 
        [SerializeField] private float pauseTime;
        public override MusicLogic GetMusicLogic(MusicManager manager)
        {
            return new SingleRepeatMusicLogic(manager, this);
        }

        public MusicData MusicData => musicData;
        public float PauseTime => pauseTime;
    }

    public class SingleRepeatMusicLogic : MusicLogic
    {
        private readonly SingleRepeatMusicLogicData data;
        private Coroutine musicCoroutine;
        public SingleRepeatMusicLogic(MusicManager manager, SingleRepeatMusicLogicData data) : base(manager)
        {
            this.data = data;
        }

        public override void Start()
        {
            manager.Play(data.MusicData);
            musicCoroutine = manager.StartCoroutine(MusicCoroutine());
        }

        public override void Exit()
        {
            if(musicCoroutine != null)
                manager.StopCoroutine(musicCoroutine);
            manager.Stop();
        }

        private IEnumerator MusicCoroutine()
        {
            while (true)
            {
                manager.Play(data.MusicData);

                while (manager.IsPlaying)
                    yield return new WaitForFixedUpdate();
            }
        }
    }
}
