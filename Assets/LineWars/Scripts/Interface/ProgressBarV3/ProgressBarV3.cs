using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarV3 : MonoBehaviour
{
    [SerializeField] private Color hpColor;

    [SerializeField] private Color armorColor;

    [SerializeField] private GameObject prefabElement;

    [SerializeField] private int maxHp;

    [SerializeField] private RectTransform areaElements;

    [SerializeField] private int hpCount;

    [SerializeField] private int armorCount;

    [SerializeField] [Range(1f, 100f)] private float SpeedChenges;

    [SerializeField] private float widthElement;

    [SerializeField] private float horizontalBoard;

    private const float frameMax = 1000; 

    private List<RectTransform> elements = new List<RectTransform>();

    public void SetHp(int value) => SetValue(value, armorCount, Segment.Hp);

    public void SetArmor(int value) => SetValue(hpCount, value, Segment.Armor);

    public void SetBar(int countHp, int countArmor) => SetValue(countHp, countArmor, Segment.All);

    

    private void Awake()
    {
        CreateNewElement(maxHp);
        SetBar(hpCount, armorCount);
    }

    void SetValue(int valueHp, int valueArmor, Segment typeChange)
    {
        if (valueHp >= 0 || valueArmor >= 0)
        {
            int changeValue = typeChange == Segment.Hp ? valueHp : typeChange == Segment.Armor ? valueArmor : valueHp + valueArmor;
            int currentValue = typeChange == Segment.Hp ? hpCount : typeChange == Segment.Armor ? armorCount : hpCount + armorCount;
            int difference = changeValue - currentValue;
            int startInterval = difference >= 0 ? currentValue : changeValue;
            int endInterval = difference >= 0 ? changeValue : currentValue;
            if (typeChange == Segment.Armor)
            {
                startInterval += valueHp;
                endInterval += valueHp;
            }
            float finalWidth = 0;
            if (valueHp > 0 || valueArmor > 0)
                finalWidth = (areaElements.GetComponent<RectTransform>().rect.width - 2 * horizontalBoard) / (valueHp + valueArmor);

            if (valueHp + valueArmor > elements.Count)
                CreateNewElement(valueHp + valueArmor - elements.Count);

            StartCoroutine(ChangeValue(startInterval, endInterval, finalWidth, valueHp, valueArmor, difference > 0));
        }
    }

    private void CreateNewElement(int count)
    {
        for (int i = 0; i != count; i++)
        {
            var element = Instantiate(prefabElement, areaElements.transform);
            elements.Add(element.GetComponent<RectTransform>());
        }
    }

    void SetSizeWithInterval(int startInterval, int endInterval, float widthInInterval, float widthOutInterval)
    {
        float leftDistance = horizontalBoard;
        for (int i = 0; i != elements.Count; i++)
        {
            elements[i].localPosition = new Vector3(leftDistance, elements[i].localPosition.y, elements[i].localPosition.z);

            var interval = i >= startInterval && i < endInterval ? widthInInterval : widthOutInterval;

            var rectTransform = elements[i].GetComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(interval, areaElements.rect.height);
            leftDistance += interval;
        }
    }

    void SetExtreme()
    {
        for (int i = 0; i != elements.Count; i++)
        {
            var isOutside = i < hpCount + armorCount;
            elements[i].gameObject.SetActive(isOutside);
            if (i < hpCount + armorCount)
                elements[i].GetComponent<Image>().color = i < hpCount ? hpColor : armorColor;
        }
    }

    IEnumerator ChangeValue(int startInterval, int endInterval, float finalWidth, int finalHp, int finalArmor, bool increace)
    {
        float step = frameMax / SpeedChenges;
        float stepInInterval = increace ? finalWidth / step : -1 * widthElement / step;
        float stepOutInterval = (finalWidth - widthElement) / step;
        float widthInInterval = increace ? 0 : widthElement;
        float widthOutInterval = widthElement;

        hpCount = finalHp;
        armorCount = finalArmor;

        if(increace)
            SetExtreme();
        
        for (float i = 0; i <= frameMax; i += SpeedChenges)
        {
            SetSizeWithInterval(startInterval, endInterval, widthInInterval, widthOutInterval);

            if (increace)
            {
                if (widthInInterval < finalWidth)
                    widthInInterval += stepInInterval;
                if (widthOutInterval > finalWidth)
                    widthOutInterval += stepOutInterval;
            }
            else
            {
                if (widthInInterval > 0)
                    widthInInterval += stepInInterval;
                if (widthOutInterval < finalWidth)
                    widthOutInterval += stepOutInterval;
            }
                
            yield return new WaitForFixedUpdate();
        }

        SetExtreme();
        SetSizeWithInterval(startInterval, endInterval, finalWidth, finalWidth);

        widthElement = finalWidth;
    }

    enum Segment 
    {
        Hp,
        Armor,
        All
    }
}
