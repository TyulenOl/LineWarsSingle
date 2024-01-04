using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ProgressBarV2 : MonoBehaviour
{
    [SerializeField]
    private Slider Slider;

    [SerializeField][Range(0f, 100f)] 
    private float SpeedChenges;

    private float FramesChange;

    private void RefreshFrameChange() => FramesChange = 101 - SpeedChenges;

    public void SetValue(float value)
    {
        value = Mathf.Clamp(value, Slider.minValue, Slider.maxValue);
        var difference = -1 * (Slider.value - (Slider.maxValue - value));
        RefreshFrameChange();
        StartCoroutine(ChangeValue(difference));
    }

    public float GetValue()
    {
        return Slider.maxValue - Slider.value;
    }

    IEnumerator ChangeValue(float difference)
    {
        float absValue = Mathf.Abs(difference);
        float startValue = Slider.value;
        for (float i = 0; i < absValue; i += absValue / FramesChange)
        {
            Slider.value += difference / FramesChange;
            yield return new WaitForFixedUpdate();
        }

        Slider.value = startValue + difference;
    }
}
