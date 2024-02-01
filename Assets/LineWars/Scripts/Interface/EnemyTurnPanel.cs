using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurnPanel : MonoBehaviour
{
    private float alphaDecreaseModifier;

    [SerializeField] private Image lizardsImage;
    [SerializeField] private Image rusImage;

    private bool isCoroutineActive;

    public bool IsCoroutineActive
    {
        get => isCoroutineActive;
        set
        {
            isCoroutineActive = value;
            if (value)
                RestoreDefaults();
            else
                Hide();
        }
    }

    private void Hide()
    {
        lizardsImage.gameObject.SetActive(false);
        StartCoroutine(HideCoroutine());
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