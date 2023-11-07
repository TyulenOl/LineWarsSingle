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
        [SerializeField] private TMP_Text missionStatus;
        [SerializeField] private Button startButton;

        private SceneName sceneToLoad;

        private void Awake()
        {
            CheckValid();
        }

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

        private void CheckValid()
        {
            if (missionName == null)
                Debug.LogError($"{nameof(missionName)} is null on {name}");

            if (missionDescription == null)
                Debug.LogError($"{nameof(missionDescription)} is null on {name}");

            if (missionImage == null)
                Debug.LogError($"{nameof(missionImage)} is null on {name}");

            //if (missionStatus == null)
            //Debug.LogError($"{nameof(missionStatus)} is null on {name}");

            if (startButton == null)
                Debug.LogError($"{nameof(startButton)} is null on {name}");
        }

        public void Initialize(MissionState state)
        {
            var data = state.missionData;
            missionName.text = data.MissionName;
            missionDescription.text = data.MissionDescription;
            missionImage.sprite = data.MissionImage;

            //missionStatus.text = state.isCompleted ? @"<color=green>Завершена</color>" : "Не пройдена";

            sceneToLoad = data.SceneToLoad;
        }
    }
}