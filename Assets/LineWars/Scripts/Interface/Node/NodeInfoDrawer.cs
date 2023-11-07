using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeInfoDrawer : MonoBehaviour
{
    [SerializeField] private TMP_Text incomeText;
    [SerializeField] private TMP_Text capturingText;

    [SerializeField] private GameObject inCapturedNodeRectTransform;
    [SerializeField] private GameObject capturedNodeRectTransform;

    public void Capture()
    {
        inCapturedNodeRectTransform.gameObject.SetActive(false);
        capturedNodeRectTransform.gameObject.SetActive(true);
    }

    public void ReDrawCapturingInfo(int capturingMoney)
    {
        capturingText.text = $"+{capturingMoney}";
    }

    public void ReDrawIncomeInfo(float incomeMoney)
    {
        incomeText.text = $"+{incomeMoney}";
    }
}