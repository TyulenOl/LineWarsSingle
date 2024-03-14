using System;
using System.Globalization;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Infrastructure
{
    public class SpecialOffertTrigger: Trigger
    {
        private bool triggered;
        public override bool Triggered => triggered;
        public override event Action TriggeredEvent;

        [SerializeField] private string id;
        [SerializeField] private string startDate;
        [SerializeField] private string endDate;

        private static GameRoot GameRoot => GameRoot.Instance;
        private static UserInfoController UserController => GameRoot.Instance?.UserController;

        private void Start()
        {
            if (GameRoot != null)
            {
                if (GameRoot.GameReady)
                    CheckSpecialOffert();
                else
                    GameRoot.OnGameReady += CheckSpecialOffert;
            }
        }
        
        private void CheckSpecialOffert()
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning($"{nameof(SpecialOffertTrigger)} id is null or empty!");
                return;
            }
            
            var startDateAvailable = DateTime.TryParse(
                startDate,
                new CultureInfo("ru-Ru"),
                DateTimeStyles.None,
                out var start);

            if (!startDateAvailable)
            {
                Debug.LogError($"{nameof(SpecialOffertTrigger)} {nameof(startDate)} is not available!");
            }
            
            var endDateAvailable = DateTime.TryParse(
                endDate,
                new CultureInfo("ru-Ru"),
                DateTimeStyles.None,
                out var end);
            
            if (!endDateAvailable)
            {
                Debug.LogError($"{nameof(SpecialOffertTrigger)} {nameof(endDate)} is not available!");
            }

            if (startDateAvailable && endDateAvailable && start < end)
            {
                var now = DateTime.Now;
                if (start < now && now < end && !UserController.KeyToBool[id])
                {
                    triggered = true;
                    TriggeredEvent?.Invoke();
                    UserController.KeyToBool[id] = true;
                }
            }
        }
    }
}