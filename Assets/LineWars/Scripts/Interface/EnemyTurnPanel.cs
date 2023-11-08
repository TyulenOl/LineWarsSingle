using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurnPanel : MonoBehaviour
{
    private float alphaDecreaseModifier;

    [SerializeField] private Image lizardsImage;
    [SerializeField] private Image rusImage;

    private const float MIN_ALPHA = 130f;
    private const float ALPHA_DECREASE_MODIFIER = -3f;

    private bool isCoroutinActive;

    public bool IsCoroutinActive
    {
        get => isCoroutinActive;
        set
        {
            isCoroutinActive = value;
            if (value)
                RestoreDefaults();
            else
                Hide();
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

    private void Hide()
    {
        lizardsImage.gameObject.SetActive(false);
        StartCoroutine(HideCoroutine());
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
        lizardsImage.color = new Color(lizardsImage.color.r, lizardsImage.color.g, lizardsImage.color.b,
            resultAlpha / 255f);
    }

    IEnumerator HideCoroutine()
    {
        rusImage.gameObject.SetActive(true);
        rusImage.color = new Color(rusImage.color.r, rusImage.color.g, rusImage.color.b, 1);
        var resultAlpha = 255f;
        while (rusImage.gameObject.activeInHierarchy && rusImage.color.a > 0.1)
        {
            resultAlpha -= 2f;
            yield return new WaitForSeconds(0.01f);
            rusImage.color = new Color(rusImage.color.r, rusImage.color.g, rusImage.color.b, resultAlpha / 255f);
        }

        rusImage.gameObject.SetActive(false);
    }

    private void RestoreDefaults()
    {
        lizardsImage.color = new Color(lizardsImage.color.r, lizardsImage.color.g, lizardsImage.color.b, 1);
        lizardsImage.gameObject.SetActive(true);
        rusImage.gameObject.SetActive(false);
    }
}