using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Education
{
    public class Education : MonoBehaviour
    {
        [SerializeField] private CommandsManager commandsManager;
        [SerializeField] private CameraController cameraController;

        private void Start()
        {
        }


#if UNITY_EDITOR
        [Header("DEBUG")]
        [SerializeField] private bool commandsManagerActiveSelf;
        private void Update()
        {
            if (commandsManagerActiveSelf != CommandsManager.Instance.ActiveSelf)
            {
                if (commandsManagerActiveSelf)
                    commandsManager.Activate();
                else
                    commandsManager.Deactivate();
            }
        }
#endif
    }
}