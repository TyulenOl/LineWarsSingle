using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(fileName = "New Random Music Logic", menuName = "Audio/Plugin/Music Logic/Random")]
    public class PluginRandomMusicLogicData : PluginMusicLogicData
    {
        [SerializeField] private List<PluginMusicData> musicDataList;
        [SerializeField] private float pauseTime;
        public override PluginMusicLogic GetMusicLogic(PluginMusicManager manager)
        {
            return new PluginRandomMusicLogic(manager, this);
        }
        public IReadOnlyList<PluginMusicData> MusicDataList => musicDataList;
        public float PauseTime => pauseTime;
    }

    public class PluginRandomMusicLogic : PluginMusicLogic
    {
        private readonly PluginRandomMusicLogicData data;
        private bool isPlaying;
        private Coroutine musicCoroutine;

        public PluginRandomMusicLogic(PluginMusicManager manager, PluginRandomMusicLogicData data) : base(manager)
        {
            this.data = data;
        }

        public override void Start()
        {
            isPlaying = true;
            musicCoroutine = manager.StartCoroutine(PlayRandom(-1));
        }

        public override void Exit()
        {
            isPlaying = false;
            manager.StopCoroutine(musicCoroutine);
            manager.Stop();
        }

        private IEnumerator PlayRandom(int prevMusicId)
        {
            var canPlayAuthorMusic = MusicSettings.Instance == null || MusicSettings.Instance.EnableAuthorMusic;
            var possibleMusics = data.MusicDataList
                .Where(x => canPlayAuthorMusic || !x.IsAuthorMusic)
                .ToArray();
            if (possibleMusics.Length == 0)
                possibleMusics = data.MusicDataList.ToArray();

            int musicId = -1;
            while (true)
            {
                var newId = Random.Range(0, possibleMusics.Length);
                if (newId != prevMusicId)
                {
                    musicId = newId;
                    break;
                }
            }
            manager.Stop();
            yield return new WaitForSeconds(data.PauseTime);
            manager.MusicFinished.AddListener(OnMusicEnd);
            manager.Play(possibleMusics[musicId]);

            void OnMusicEnd(PluginMusicManager _, PluginMusicData musicData)
            {
                if (!isPlaying) return;
                manager.MusicFinished.RemoveListener(OnMusicEnd);
                musicCoroutine = manager.StartCoroutine(PlayRandom(musicId));
            }
        }
    }
}

