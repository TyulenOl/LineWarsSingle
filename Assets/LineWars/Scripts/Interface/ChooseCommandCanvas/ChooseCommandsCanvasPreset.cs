using LineWars.Controllers;
using UnityEngine;

namespace LineWars
{
    public class ChooseCommandsCanvasPreset : MonoBehaviour
    {
        [SerializeField] private ReselectPreset reselectPreset;
        [SerializeField] private NoReselectPreset noReselectPreset;
        public void ReDraw(OnWaitingCommandMessage message)
        {
            gameObject.SetActive(true);
            if (reselectPreset != null && message.CanReselect)
            {
                noReselectPreset.gameObject.SetActive(false);
                reselectPreset.ReDraw(message);
            }
            else
            {
                reselectPreset?.gameObject.SetActive(false);
                noReselectPreset.ReDraw(message);
            }
        }
    }
}
