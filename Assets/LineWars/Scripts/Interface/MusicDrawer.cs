using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class MusicDrawer: MonoBehaviour
    {
        private static readonly int animateId = Animator.StringToHash("animate");
        
        [SerializeField] private Animator animator;
        [SerializeField] private TMP_Text author;
        [SerializeField] private TMP_Text soundName;
        [SerializeField] private TwoStringEventChannel listenedMusicChangedChannel;

        private string currentTitle;
        private string currentArtist;
        

        private void OnEnable()
        { 
            listenedMusicChangedChannel.Raised += MusicChanged;
        }

        private void OnDisable()
        {
            listenedMusicChangedChannel.Raised -= MusicChanged;
        }

        private void MusicChanged(string title, string artist)
        {
            if(title != currentTitle || artist != currentArtist)
            {
                currentTitle = title;
                currentArtist = artist;
                Animate(title, artist);
            }
        }

        private void Animate(string title, string artist)
        {
            var haveName = !string.IsNullOrEmpty(title);
            var haveAuthor = !string.IsNullOrEmpty(artist);

            author.text = artist;
            soundName.text = title;

            if (haveName && haveAuthor)
                animator.SetTrigger(animateId);
        }
    }
}