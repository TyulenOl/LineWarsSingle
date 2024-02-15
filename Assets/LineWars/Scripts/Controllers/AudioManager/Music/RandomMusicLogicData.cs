using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(fileName = "New Random Music Logic", menuName = "Audio/Music Logic/Random")]
    public class RandomMusicLogicData : MusicLogicData
    {
        [SerializeField] private List<MusicData> musicDataList;
        [SerializeField] private float pauseTime;
        public override MusicLogic GetMusicLogic(MusicManager manager)
        {
            return new RandomMusicLogic(manager, this);
        }
        public IReadOnlyList<MusicData> MusicDataList => musicDataList;
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
            manager.Stop();
        }

        private IEnumerator MusicCoroutine()
        {
            var canPlayAuthorMusic = MusicSettings.Instance == null || MusicSettings.Instance.EnableAuthorMusic;
            var possibleMusics = data.MusicDataList
                .Where(x => canPlayAuthorMusic || !x.IsAuthorMusic)
                .ToArray();
            if (possibleMusics.Length == 0)
                possibleMusics = data.MusicDataList.ToArray();
            
            var musicId = -1;
            while (true)
            {
                while (true)
                {
                    var newId = Random.Range(0, possibleMusics.Length);
                    if (newId != musicId)
                    {
                        musicId = newId;
                        break;
                    }
                }
                manager.Stop();
                yield return new WaitForSeconds(data.PauseTime);
                manager.Play(possibleMusics[musicId]);
                yield return new WaitForSeconds(possibleMusics[musicId].AudioClip.length);
            }
        }
    }
}

