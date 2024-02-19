using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(Button))]
    [DisallowMultipleComponent]
    public class ButtonPlaySFX : PlaySFX
    {
        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Play);
        }
    }
}
