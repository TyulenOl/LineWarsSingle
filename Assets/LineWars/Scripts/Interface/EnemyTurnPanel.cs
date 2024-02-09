using System;
using System.Collections;
using LineWars.Interface;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurnPanel : MonoBehaviour
{
    [SerializeField] private float alphaDecreaseModifier;
    [SerializeField] private float minAlpha;
    
    [SerializeField] private CanvasGroup lizardsImage;
    [SerializeField] private CanvasGroup rusImage;
    [SerializeField] private CanvasGroup economyImage;
    
    private CanvasGroup currentCanvasGroup;

    private CanvasGroup CurrentCanvasGroup
    {
        get => currentCanvasGroup;

        set
        {
            value.alpha = currentCanvasGroup.alpha;
            currentCanvasGroup.gameObject.SetActive(false);
            currentCanvasGroup = value;
            currentCanvasGroup.gameObject.SetActive(true);
        }
    }
    
    private bool isCoroutineActive;

    private void Awake()
    {
        currentCanvasGroup = rusImage;
    }

    private void FixedUpdate()
    {
        ChangeAlpha(currentCanvasGroup);
    }

    public void SetTurn(bool isEnemyTurn)
    {
        CurrentCanvasGroup = isEnemyTurn ? lizardsImage : rusImage;
    }

    public void SetEconomyTurn()
    {
        CurrentCanvasGroup = economyImage;
    }
    
    private void ChangeAlpha(CanvasGroup canvasGroup)
    {
        var currentAlpha = (float)Math.Round(canvasGroup.alpha * 255);
        if (currentAlpha >= 255)
        {
            alphaDecreaseModifier = -Math.Abs(alphaDecreaseModifier);
        }
        else if (currentAlpha <= minAlpha)
        {
            alphaDecreaseModifier = Math.Abs(alphaDecreaseModifier);
        }

        canvasGroup.alpha = (currentAlpha + alphaDecreaseModifier)/255;
    }
}