using UnityEngine;
using UnityEngine.UI;
using LineWars.Controllers;

namespace LineWars.Interface
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private VolumeType channelType;
        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Start()
        {
            var volume = VolumeUpdater.Instance.GetVolume(channelType);
            slider.value = volume;
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float value)
        {
            VolumeUpdater.Instance.SetVolume(channelType, value);
        }
    }
}

