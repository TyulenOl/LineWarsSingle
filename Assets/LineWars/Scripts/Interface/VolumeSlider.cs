using UnityEngine;
using UnityEngine.UI;
using LineWars.Controllers;

namespace LineWars.Interface
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private VolumeType channelType;
        [SerializeField] private Scrollbar scrollbar;


        private void Start()
        {
            var volume = VolumeUpdater.Instance.GetVolume(channelType);
            if (scrollbar == null)
                Debug.Log(name);
            scrollbar.value = volume;
            scrollbar.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float value)
        {
            VolumeUpdater.Instance.SetVolume(channelType, value);
        }
    }
}