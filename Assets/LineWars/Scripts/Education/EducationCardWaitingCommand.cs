using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public class EducationCardWaitingCommand : EducationCardBase
    {
        private void OnEnable()
        {
            CommandsManager.Instance.CommandIsExecuted += OnCommandIsExecuted;
        }

        private void OnCommandIsExecuted(ICommand obj)
        {
            carousel.Next();
        }
        
        private void OnDisable()
        {
            CommandsManager.Instance.CommandIsExecuted -= OnCommandIsExecuted;
        }
    }
}