using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InfiniteTimer : MonoBehaviour
{
    [SerializeField, Min(0)] private int startValue = 1000000;
    [SerializeField, Min(1)] private int speed = 1;
    [SerializeField] private TMP_Text text;
    
        
    private void Awake()
    {
        var currentValue = startValue;
        text.text = currentValue.ToString();
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            while (currentValue >= 0)
            {
                text.text = currentValue.ToString();
                currentValue -= speed;
                yield return new WaitForSeconds(1);

            }
        }
    }
}
