using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using UnityEngine;

public class AutoScalerHard : MonoBehaviour
{
    [SerializeField] private RectTransform resizableTransform;
    private void Update()
    {
        var canvas = MainCanvas.Instance.Canvas;
        var xCoeff = canvas.pixelRect.width / 2400;
        var yCoeff = canvas.pixelRect.height / 1080;
        var coef = Math.Max(xCoeff, yCoeff);
        var localScale = resizableTransform.transform.localScale;
        localScale = new Vector3(
            coef,
            coef,
            localScale.z);
        resizableTransform.transform.localScale = localScale;
    }
}
