using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(fileName = "New Random Music Logic", menuName = "Audio/Music Logic/Random")]
    public class RandomMusicLogicData : MusicLogicData
    {
        [SerializeField] private List<AudioClip> musicList;
        [SerializeField] private float pauseTime;
        public override MusicLogic GetMusicLogic(MusicManager manager)
        {
            return new RandomMusicLogic(manager, this);
        }

        public IReadOnlyList<AudioClip> MusicList => musicList;
        public float PauseTime => pauseTime;
    }

    public class RandomMusicLogic : MusicLogic
    {
        private readonly RandomMusicLogicData data;
        private Coroutine musicCoroutine;
        public RandomMusicLogic(MusicManager manager, RandomMusicLogicData data) : base(manager)
        {
            this.data = data;
        }

        public override void Start()
        {
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
            var musicId = -1;
            while (true)
            {
                while (true)
                {
                    var newId = Random.Range(0, data.MusicList.Count);
                    if (newId != musicId)
                    {
                        musicId = newId;
                        break;
                    }
                }
                manager.Source.Stop();
                yield return new WaitForSeconds(data.PauseTime);
                manager.Source.clip = data.MusicList[musicId];
                manager.Source.Play();
                yield return new WaitForSeconds(data.MusicList[musicId].length);
            }
        }
    }
}

