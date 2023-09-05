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
    [SerializeField] private float rotationSpeed;
    
    [SerializeField] private Image clockImage;
    [SerializeField] private TMP_Text text;

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
            clockImage.gameObject.transform.Rotate(new Vector3(0,0,rotationSpeed));
        }
    }

    private void ChangeAlpha()
    {
        var currentAlpha = text.color.a * 255;
        if (currentAlpha >= 255)
        {
            alphaDecreaseModifier = -Math.Abs(alphaDecreaseModifier);
        }
        else if (currentAlpha <= MIN_ALPHA)
        {
            alphaDecreaseModifier = Math.Abs(alphaDecreaseModifier);
        }

        var resultAlpha = currentAlpha + alphaDecreaseModifier;
        text.color = new Color(text.color.r, text.color.g, text.color.b, resultAlpha/255f);
    }
    
    private void RestoreDefaults()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        clockImage.gameObject.transform.localRotation = new Quaternion(0,0,0,0);
        gameObject.SetActive(true);
    }
}
