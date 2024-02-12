using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class MissionInfoUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text missionName;
        [SerializeField] private TMP_Text missionDescription;
        [SerializeField] private Image missionImage;
        [SerializeField] private Button startButton;
        [SerializeField] private Button openDeckPanelButton;
        [SerializeField] private TMP_Text onButtonText;

        private SceneName sceneToLoad;

        private void OnEnable()
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveListener(OnStartButtonClick);
        }

        private void OnStartButtonClick()
        {
            SceneTransition.LoadScene(sceneToLoad);
        }
        
        public void Redraw(MissionData data, string onButtonText)
        { 
            gameObject.SetActive(true);
            openDeckPanelButton.gameObject.SetActive(true);
            missionName.text = data.MissionName;
            missionDescription.text = data.MissionDescription;
            missionImage.sprite = data.MissionImage;
            sceneToLoad = data.SceneToLoad;
            this.onButtonText.text = onButtonText;
        }
    }
}