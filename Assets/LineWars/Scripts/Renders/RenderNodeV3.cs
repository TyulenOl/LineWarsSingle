using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RenderNodeV3 : MonoBehaviour
{
    [SerializeField] private SpriteRenderer hiddenNodeSpriteRenderer;
    [SerializeField] private List<Sprite> hiddenNodeSprites;
    [SerializeField] [Range(0, 1)] private float visibility;
    public float Visibility
    {
        get => visibility;
        private set
        {
            visibility = value;
            hiddenNodeSpriteRenderer.color = hiddenNodeSpriteRenderer.color.WithAlpha(Mathf.Pow(1 - visibility, 2));
        }
    }

    private void Awake()
    {
        Visibility = 0;
        if (hiddenNodeSpriteRenderer.sprite != null)
            return;
        hiddenNodeSprites = hiddenNodeSprites.Where(x => x != null).ToList();
        if (hiddenNodeSprites.Count != 0)
            hiddenNodeSpriteRenderer.sprite = hiddenNodeSprites[Random.Range(0, hiddenNodeSprites.Count)];
    }

    public void SetVisibilityGradually(float value)
    {
        if (Mathf.Abs(value - Visibility) < 0.00001f)
            return;
        value = Mathf.Clamp(0, value, 1);
        StartCoroutine(AnimateVisibility(value));
    }

    public void SetVisibilityInstantly(float value)
    {
        if (Mathf.Abs(value - Visibility) < 0.00001f)
            return;
        Visibility = Mathf.Clamp(0, value, 1);
    }


    private IEnumerator AnimateVisibility(float targetVal)
    {
        float startingTime = Time.time;
        float startingVal = Visibility;
        float lerpVal = 0.0f;
        while (lerpVal < 1.0f)
        {
            lerpVal = (Time.time - startingTime) / 1.0f;
            Visibility = Mathf.Lerp(startingVal, targetVal, lerpVal);
            yield return null;
        }
        
        Visibility = targetVal;
    }
}