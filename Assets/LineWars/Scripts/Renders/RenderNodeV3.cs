using System;
using System.Collections;
using UnityEngine;

public class RenderNodeV3: MonoBehaviour
{
    [Range(0,1)] public float visibility;
    public float Visibility => visibility;

    public void SetVisibility(float value)
    {
        value = Math.Min(Math.Max(0, value), 1);
        StartCoroutine(AnimateVisibility(value));
    }
    
    
    private IEnumerator AnimateVisibility(float targetVal)
    {
        float startingTime = Time.time;
        float startingVal = visibility;
        float lerpVal = 0.0f;
        while(lerpVal < 1.0f)
        {
            lerpVal = (Time.time - startingTime) / 1.0f;
            visibility = Mathf.Lerp(startingVal, targetVal, lerpVal);
            yield return null;
        }
        visibility = targetVal;
    }
}