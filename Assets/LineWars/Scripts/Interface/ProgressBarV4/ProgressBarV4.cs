using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarV4 : MonoBehaviour
{
    [SerializeField] private GameObject prefabElement;
    [SerializeField] private RectTransform areaElement;
    [SerializeField] private RectTransform areaInterval;
    [SerializeField] private float widthElement;
    [SerializeField] private float widthInterval;
    [SerializeField][Range(1f, 100f)] private float speedTimeChenges;
    private int[] countValues;
    private List<RectTransform> elements = new List<RectTransform>();
    private List<RectTransform> intervals = new List<RectTransform>();
    [SerializeField] private List<Color> colors = new List<Color>();
    private const float frameMax = 1000;
    private bool coroutineFlag = false;



    [SerializeField] private float?[] widthArea;
    [SerializeField] private float?[] fff;




    public void Init(int countElement, params int[] values)
    {
        countValues = new int[countElement];
        int[] newValue = new int[countElement];
        for (int i = 0; i != countElement; i++)
        {
            newValue[i] = values[i];
        }
        CreateNewElement(countElement, Area.element);
        ChangeValue(newValue);
    }

    public void SetValue(params (int, int)[] values)
    {
        int[] result = countValues;
        foreach (var value in values)
        {
            if (value.Item1 < 0 || value.Item2 < 0)
                throw new UnityException("Wrong value");
            result[value.Item1] = value.Item2;
        }
        ChangeValue(result);
    }

    private int GetSumCountValues(int[] values)
    {
        int result = 0;
        foreach (var count in values)
            result += count;
        return result;
    }

    private void CreateNewElement(int count, Area area)
    {
        RectTransform rect = area == Area.element ? areaElement : areaInterval;
        for (int i = 0; i != count; i++)
        {
            var element = Instantiate(prefabElement, rect.transform);
            if (area == Area.element)
            {
                elements.Add(element.GetComponent<RectTransform>());
                elements[i].GetComponent<Image>().color = colors[i];
            }
            else
            {
                element.transform.GetComponent<Image>().color = new Color(0, 0, 0, 255);
                var elementRect = element.GetComponent<RectTransform>();
                elementRect.sizeDelta = new Vector2(widthInterval, areaInterval.rect.height);
                intervals.Add(elementRect);
            }
        }
    }

    private void ChangeValue(int[] values)
    {
        int startCountValue = GetSumCountValues(countValues);
        int finishCountValue = GetSumCountValues(values);
        float newWidthElement = areaElement.rect.width / finishCountValue;

        List<(float, float)> elementWidth = new List<(float, float)>();
        List<(float, float)> intervalValues = new List<(float, float)>();

        for (int i = 0; i != countValues.Length; i++)
        {
            elementWidth.Add((widthElement * countValues[i], newWidthElement * values[i]));
            var count = startCountValue > finishCountValue ? countValues[i] : values[i];
            for (int j = 0; j != count; j++)
                intervalValues.Add((j >= countValues[i] ? 0 : widthElement, j < values[i] ? newWidthElement : 0));
        }
        widthElement = newWidthElement;
        countValues = values;
        if (!coroutineFlag)
            StartCoroutine(AnimationChange(elementWidth, intervalValues));
    }

    private void SetSizeObjectsArea(List<float> stepsWidth, List<float> stepsValue)
    {
        float leftDistance = 0;
        for (int i = 0; i != stepsWidth.Count; i++)
        {
            var element = elements[i];
            element.localPosition = new Vector3(leftDistance, element.localPosition.y, element.localPosition.z);
            var newWidth = element.sizeDelta.x + stepsWidth[i];
            element.sizeDelta = new Vector2(newWidth, element.rect.height);
            leftDistance += newWidth;
        }
        leftDistance = 0;
        for (int i = 0; i != stepsValue.Count; i++)
        {
            var interval = intervals[i];
            leftDistance += intervals[0].localPosition.x + stepsValue[i];
            leftDistance = leftDistance < 0 ? 0 : leftDistance > areaElement.rect.width ? areaElement.rect.width : leftDistance;
            Debug.Log(leftDistance);
            interval.localPosition = new Vector3(leftDistance, interval.localPosition.y, interval.localPosition.z);
        }
    }
    private void SetExtremeValue(bool flag, List<(float, float)> widthsElement, List<(float, float)> intervalsValue)
    {
        int sumCountValues = GetSumCountValues(countValues);
        if (sumCountValues > intervals.Count)
            CreateNewElement(sumCountValues - intervals.Count, Area.interval);
        float leftDistance = 0;
        for (int i = 0; i != widthsElement.Count; i++)
        {
            var element = elements[i];
            element.gameObject.SetActive(true);
            element.localPosition = new Vector3(leftDistance, element.localPosition.y, element.localPosition.z);
            float width = flag ? widthsElement[i].Item2 : widthsElement[i].Item1;
            element.sizeDelta = new Vector2(width, areaElement.rect.height);
            leftDistance += width;
        }

        leftDistance = 0;

        for (int i = 0; i != intervalsValue.Count; i++)
        {
            var interval = intervals[i];
            if (flag && intervalsValue[i].Item2 == 0)
                interval.gameObject.SetActive(false);
            else
                interval.gameObject.SetActive(true);

            float distance = flag ? intervalsValue[i].Item2 : intervalsValue[i].Item1;
            leftDistance += distance;
            interval.localPosition = new Vector3(leftDistance, interval.localPosition.y, interval.localPosition.z);
            interval.sizeDelta = new Vector2(widthInterval, areaInterval.rect.height);
        }
    }

    private IEnumerator AnimationChange(List<(float, float)> widthsElement, List<(float, float)> intervalValues)
    {
        coroutineFlag = true;
        float steps = frameMax / speedTimeChenges;
        List<float> stepsWidth = new List<float>();
        List<float> stepsInterval = new List<float>();
        for (int i = 0; i != widthsElement.Count; i++)
            stepsWidth.Add((widthsElement[i].Item2 - widthsElement[i].Item1) / steps);

        for (int i = 0; i != intervalValues.Count; i++)
            stepsInterval.Add((intervalValues[i].Item2 - intervalValues[i].Item1) / steps);

        SetExtremeValue(false, widthsElement, intervalValues);
        for (float i = 0; i <= frameMax; i += speedTimeChenges)
        {
            SetSizeObjectsArea(stepsWidth, stepsInterval);
            yield return new WaitForFixedUpdate();
        }

        SetExtremeValue(true, widthsElement, intervalValues);
        coroutineFlag = false;
    }

    private enum Area
    {
        element,
        interval
    }
}
