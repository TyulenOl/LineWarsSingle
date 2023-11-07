using System;
using System.Linq;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class CompanyElementUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text companyName;
        [SerializeField] private TMP_Text companyDescription;
        [SerializeField] private Image companyImage;
        [SerializeField] private TMP_Text missionsProgress;
        [SerializeField] private Button companyElementButton;

        private Action<CompanyState> clicked;
        private CompanyState currentState;

        private void Awake()
        {
            CheckValid();
        }

        private void OnEnable()
        {
            if (companyElementButton != null)
                companyElementButton.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            if (companyElementButton != null)
                companyElementButton.onClick.RemoveListener(OnClick);
        }

        private void OnClick() => clicked?.Invoke(currentState);


        private void CheckValid()
        {
            if (companyName == null)
                Debug.LogError($"{nameof(companyName)} is null on {name}");

            if (companyDescription == null)
                Debug.LogError($"{nameof(companyDescription)} is null on {name}");

            if (companyImage == null)
                Debug.LogError($"{nameof(companyImage)} is null on {name}");

            if (missionsProgress == null)
                Debug.LogError($"{nameof(missionsProgress)} is null on {name}");

            //if (companyElementButton == null)
            //Debug.LogError($"{nameof(companyElementButton)} is null on {name}");
        }

        public void Initialize(CompanyState companyState, Action<CompanyState> clicked)
        {
            if (companyState == null) return;

            this.currentState = companyState;

            var data = companyState.companyData;

            companyName.text = data.Name;
            companyDescription.text = data.Description;
            companyImage.sprite = data.Image;

            var finishMissionsCount = companyState.missionStates
                .Count(x => x.isCompleted);

            var allMissionCount = companyState.missionStates.Count;
            missionsProgress.text = $"{finishMissionsCount}/{allMissionCount}";

            this.clicked = clicked;
        }
    }
}