using LineWars.Controllers;
using LineWars.Education;
using LineWars.Model;

namespace LineWars.Scripts.Education
{
    public class EducationCardWaitingBlessing : EducationCardBase
    {
        private void OnEnable()
        {
            CommandsManager.Instance.Activate();
            CommandsManager.Instance.BlessingCompleted += OnCommandIsExecuted;
        }

        private void OnCommandIsExecuted(BlessingMassage obj)
        {
            carousel.Next();
        }
        
        private void OnDisable()
        {
            CommandsManager.Instance.BlessingCompleted -= OnCommandIsExecuted;
        }
    }
}