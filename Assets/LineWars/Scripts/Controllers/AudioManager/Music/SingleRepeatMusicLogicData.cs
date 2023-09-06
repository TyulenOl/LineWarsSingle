using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(fileName = "new Single Repeat Music Logic", menuName = "Audio/Music Logic/Single Repeat")]
    public class SingleRepeatMusicLogicData : MusicLogicData
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private float pauseTime;
        public override MusicLogic GetMusicLogic(MusicManager manager)
        {
            return new SingleRepeatMusicLogic(manager, this);
        }

        public AudioClip Clip => clip;
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
            manager.Source.clip = data.Clip;
            musicCoroutine = manager.StartCoroutine(MusicCoroutine());
        }

        public override void Exit()
        {
            manager.StopCoroutine(musicCoroutine);
            manager.Source.Stop();
            manager.Source.clip = null;
        }

        private IEnumerator MusicCoroutine()
        {
            while (true)
            {
                manager.Source.Play();
                yield return new WaitForSeconds(data.Clip.length + data.PauseTime);
            }
        }
    }
}
