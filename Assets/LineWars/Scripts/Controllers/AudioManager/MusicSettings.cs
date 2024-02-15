using DataStructures;
using UnityEngine;

namespace LineWars.Controllers
{
    public class MusicSettings: Singleton<MusicSettings>
    {
        [SerializeField] private bool defaultEnableAuthorMusic = true;
        public bool EnableAuthorMusic { get; set; }
        
        private void OnEnable()
        {
            LoadSettings();
        }

        public void OnDisable()
        {
            Save();
        }
        
        private void LoadSettings()
        {
            if (!PlayerPrefs.HasKey(nameof(EnableAuthorMusic)))
                PlayerPrefs.GetInt(nameof(EnableAuthorMusic), defaultEnableAuthorMusic ? 1 : 0);
            EnableAuthorMusic = PlayerPrefs.GetInt(nameof(EnableAuthorMusic)) == 1;
        }

        private void Save()
        {
            PlayerPrefs.SetInt(nameof(EnableAuthorMusic), EnableAuthorMusic ? 1 : 0);
        }
    }
}