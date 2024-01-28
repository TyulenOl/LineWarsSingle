using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ProgressBar : MonoBehaviour
{

    readonly float ANIMATION_STOP_EPS = 0.005f;

    [Header("Links")]
    [SerializeField] private RectTransform[] areasRects;


    [Space(10f)]
    [Header("Settings")]
    [SerializeField] private float smoothness;
    [SerializeField]private int[] currentValues;
    
    [Space(10f)]
    [Header("Border Settings")]
    [SerializeField][Min(1)] private int initialMaxCellsCount;
    [SerializeField] private RectTransform bordersContainer;
    [SerializeField] private GameObject borderPrefab;

    
    private float?[] targetHeights;
    private RectTransform container;
    
    private List<RectTransform> borders;
    private int activeBordersCount;

    private int animatedRectsCount;



    private void FixedUpdate()
    {
        if (animatedRectsCount > 0)
        {
            var offset = 0f;
            for (var i = 0; i < areasRects.Length; i++)
            {
                var rect = areasRects[i];
                var targetHeight = targetHeights[i];
                if (targetHeight.HasValue)
                {
                    offset += targetHeight.Value / 2;

                    
                    var oldSizeDelta = rect.rect;
                    var newSizeDelta = new Vector2(oldSizeDelta.width, targetHeight.Value);

                    rect.sizeDelta = Vector2.Lerp(oldSizeDelta.size, newSizeDelta, smoothness);
                    
                    rect.localPosition = Vector2.Lerp(rect.localPosition,
                        new Vector2(rect.localPosition.x,
                        offset), smoothness);

                    
                    offset += targetHeight.Value / 2;

                    if (Mathf.Abs(rect.rect.height - targetHeight.Value) < ANIMATION_STOP_EPS)
                    {
                        targetHeights[i] = null;
                        animatedRectsCount--;
                        rect.sizeDelta = newSizeDelta;
                    }
                }
                else
                    offset += rect.rect.height;
            }
        }
    }

    protected void Initialize()
    {
        container = GetComponent<RectTransform>();
        targetHeights = new float?[areasRects.Length];
        currentValues = new int[areasRects.Length];
        for (var i = 0; i < currentValues.Length; i++)
            currentValues[i] = 1;

        borders = new List<RectTransform>();
        SpawnAdditionalBorders(initialMaxCellsCount - 1);

        RedrawValuesImmediately(currentValues);
    }

    protected void ConfigureValues(params int[] initValues)
    {
        if(initValues.Length != areasRects.Length)
            Debug.LogWarning("Progressbar | init values count doesn't match areas count");
        currentValues = initValues;
        RedrawValuesImmediately(currentValues);
    }

    protected void ChangeValue(int index, int newValue)
    {
        currentValues[index] = newValue;
        RedrawValues(currentValues);
    }

    protected void ChangeValues(params (int, int)[] newValues)
    {
        var result = currentValues;
        foreach (var pair in newValues)
        {
            result[pair.Item1] = pair.Item2;
        }
        RedrawValues(result);
    }

    private void RedrawValues(int[] newValues)
    {
        var sum = GetSum(newValues);
        currentValues = newValues;
        for (var i = 0; i < areasRects.Length; i++)
        {
            targetHeights[i] = (float)newValues[i] / sum * container.rect.height;
            animatedRectsCount++;
        }
        SetBorders(sum);
    }

    private void RedrawValuesImmediately(int[] newValues)
    {
        var sum = GetSum(newValues);
        currentValues = newValues;
        
        var offset = 0f;
        for (var i = 0; i < areasRects.Length; i++)
        {
            var rect = areasRects[i];
            var targetHeight = (float)newValues[i] / sum * container.rect.height;
            offset += targetHeight/ 2;

            rect.sizeDelta = new Vector2(rect.rect.width, targetHeight);
            rect.localPosition = new Vector2(rect.localPosition.x, offset);
                
            offset += targetHeight / 2;
        }
        SetBorders(sum);
    }

    private int GetSum(int[] values)
    {
        var sum = values.Sum();
        if (sum == 0)
            throw new ApplicationException("ProgressBar | Values sum = 0");
        return sum;
    }

    private void SetBorders(int valuesSum)
    {
        var needBorders = valuesSum - borders.Count;
        if(needBorders > 0)
            SpawnAdditionalBorders(needBorders);

        var bordersToChange = valuesSum - activeBordersCount - 1;
        if (bordersToChange > 0)
            ActivateBorders(bordersToChange);
        else if (bordersToChange < 0)
            DeactivateBorders(bordersToChange);
    }

    private void SpawnAdditionalBorders(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var borderTransform = Instantiate(borderPrefab).GetComponent<RectTransform>();
            borderTransform.SetParent(bordersContainer);
            borderTransform.gameObject.SetActive(false);
            borders.Add(borderTransform);
        }
    }

    private void ActivateBorders(int count)
    {
        var finalCount = activeBordersCount + count;
        for (var i = activeBordersCount; i < finalCount; i++)
            borders[i].gameObject.SetActive(true);
        activeBordersCount = finalCount;
    }
    
    private void DeactivateBorders(int count)
    {
        var finalIndex = activeBordersCount + count; // Count < 0
        for (var i = activeBordersCount - 1; i >= finalIndex; i--)
            borders[i].gameObject.SetActive(false);
        activeBordersCount = finalIndex;
    }

    #region Debug

    public void Increase()
    {
        currentValues = currentValues.Select(v => v + 1).ToArray();
        var result = new List<(int,int)>();
        for (var i = 0; i < currentValues.Length; i++)
        {
            result.Add((i, currentValues[i]));
        }
        ChangeValues(result.ToArray());
    }
    
    public void Decrease()
    {
        currentValues = currentValues.Select(v => v - 1).ToArray();
        var result = new List<(int,int)>();
        for (var i = 0; i < currentValues.Length; i++)
        {
            result.Add((i, currentValues[i]));
        }
        ChangeValues(result.ToArray());
    }

    public void Recalculate()
    {
        var result = new List<(int,int)>();
        for (var i = 0; i < currentValues.Length; i++)
        {
            result.Add((i, currentValues[i]));
        }
        ChangeValues(result.ToArray());
    }

    #endregion
}
