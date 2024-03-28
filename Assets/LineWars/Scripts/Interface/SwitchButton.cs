using LineWars.Infrastructure;
using UnityEngine;

namespace LineWars.Interface
{
    public class SwitchButton: ButtonClickHandler
    {
        [SerializeField] private GameObject switchObject;
        protected override void OnClick()
        {
            switchObject.SetActive(!switchObject.activeSelf);
        }
    }
}