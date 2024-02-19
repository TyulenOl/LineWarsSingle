using System;
using LineWars.Controllers;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class MusicDrawer: MonoBehaviour
    {
        private static readonly int animateId = Animator.StringToHash("animate");
        private static PluginMusicManager MusicManager => PluginMusicManager.Instance;
        
        [SerializeField] private Animator animator;
        [SerializeField] private TMP_Text author;
        [SerializeField] private TMP_Text soundName;
        private PluginMusicData current;
        

        private void OnEnable()
        {
            MusicManager.MusicChanged.AddListener(MusicChanged);
            // if (MusicManager.CurrentMusicData != null)
            //     Animate(MusicManager.CurrentMusicData);
        }

        private void OnDisable()
        {
            MusicManager.MusicChanged.RemoveListener(MusicChanged);
        }

        private void MusicChanged(PluginMusicData before, PluginMusicData after)
        {
            if (after != null)
            {
                Animate(after);
            }
        }

        private void Animate(PluginMusicData musicData)
        {
            if (current == musicData)
                return;
            
            //Debug.Log("Animate");
            current = musicData;
            
            var haveName = !string.IsNullOrEmpty(musicData.Name);
            var haveAuthor = !string.IsNullOrEmpty(musicData.Artist);

            author.text = musicData.Artist;
            soundName.text = musicData.Name;
            
            if (haveName && haveAuthor)
                animator.SetTrigger(animateId);
        }
    }
}