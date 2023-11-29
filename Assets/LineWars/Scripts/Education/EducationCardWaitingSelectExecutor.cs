using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public class EducationCardWaitingSelectExecutor : EducationCardBase
    {
        private void OnEnable()
        {
            CommandsManager.Instance.Activate();
            CommandsManager.Instance.ExecutorChanged += OnExecutorChanged;
        }

        private void OnExecutorChanged(IMonoExecutor arg1, IMonoExecutor arg2)
        {
            carousel.Next();
        }

        private void OnDisable()
        {
            CommandsManager.Instance.ExecutorChanged -= OnExecutorChanged;
        }
    }
}