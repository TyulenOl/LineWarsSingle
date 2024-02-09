using UnityEngine;

namespace LineWars.Interface
{
    public class PauseCallback: MonoBehaviour
    {
        private void OnEnable()
        {
            PauseInstaller.Pause(true);
        }

        private void OnDisable()
        {
            PauseInstaller.Pause(false);  
        }
    }
}