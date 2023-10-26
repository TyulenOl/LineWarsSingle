using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurnPanel : MonoBehaviour
{
    private float alphaDecreaseModifier;

    [SerializeField] private Image lizardsImage;

    private const float MIN_ALPHA = 130f;
    private const float ALPHA_DECREASE_MODIFIER = -3f;
    
    private bool isCoroutinActive;

    public bool IsCoroutinActive
    {
        get => isCoroutinActive;
        set
        {
            isCoroutinActive = value;
            if(value)
                RestoreDefaults();
            else
                gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        alphaDecreaseModifier = ALPHA_DECREASE_MODIFIER;
    }

    private void FixedUpdate()
    {
        if (IsCoroutinActive)
        {
            ChangeAlpha();
        }
    }

    private void ChangeAlpha()
    {
        var currentAlpha = lizardsImage.color.a * 255;
        if (currentAlpha >= 255)
        {
            alphaDecreaseModifier = -Math.Abs(alphaDecreaseModifier);
        }
        else if (currentAlpha <= MIN_ALPHA)
        {
            alphaDecreaseModifier = Math.Abs(alphaDecreaseModifier);
        }

        var resultAlpha = currentAlpha + alphaDecreaseModifier;
        lizardsImage.color = new Color(lizardsImage.color.r, lizardsImage.color.g, lizardsImage.color.b, resultAlpha/255f);
    }
    
    private void RestoreDefaults()
    {
        lizardsImage.color = new Color(lizardsImage.color.r, lizardsImage.color.g, lizardsImage.color.b, 1);
        gameObject.SetActive(true);
    }
}
