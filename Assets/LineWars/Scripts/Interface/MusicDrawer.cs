using System;
using LineWars.Controllers;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class MusicDrawer: MonoBehaviour
    {
        private static readonly int animateId = Animator.StringToHash("animate");
        private static MusicManager MusicManager => MusicManager.Instance;
        
        [SerializeField] private Animator animator;
        [SerializeField] private TMP_Text author;
        [SerializeField] private TMP_Text soundName;
        private MusicData current;
        

        private void OnEnable()
        {
            MusicManager.MusicChanged.AddListener(MusicChanged);
            if (MusicManager.CurrentMusicData != null && MusicManager.CurrentMusicData != current)
                Animate(MusicManager.CurrentMusicData);
        }

        private void OnDisable()
        {
            MusicManager.MusicChanged.RemoveListener(MusicChanged);
        }

        private void MusicChanged(MusicData before, MusicData after)
        {
            if (after != null && current != after)
            {
                Animate(after);
            }
        }

        private void Animate(MusicData musicData)
        {
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