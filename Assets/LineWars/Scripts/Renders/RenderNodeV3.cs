using System;
using System.Collections;
using UnityEngine;

public class RenderNodeV3 : MonoBehaviour
{
    [Range(0, 1)] public float visibility;
    public float Visibility => visibility;

    public void SetVisibilityGradually(float value)
    {
        if (Math.Abs(value - visibility) < 0.00001f)
            return;
        value = Mathf.Clamp(0, value, 1);
        StartCoroutine(AnimateVisibility(value));
    }

    public void SetVisibilityInstantly(float value)
    {
        if (Math.Abs(value - visibility) < 0.00001f)
            return;
        visibility = Mathf.Clamp(0, value, 1);
    }


    private IEnumerator AnimateVisibility(float targetVal)
    {
        float startingTime = Time.time;
        float startingVal = visibility;
        float lerpVal = 0.0f;
        while (lerpVal < 1.0f)
        {
            lerpVal = (Time.time - startingTime) / 1.0f;
            visibility = Mathf.Lerp(startingVal, targetVal, lerpVal);
            yield return null;
        }

        visibility = targetVal;
    }
}