using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using LineWars.Controllers;
using TMPro;
using UnityEngine;

public class RoundCounter : MonoBehaviour
{
    [SerializeField] private int counter;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Animator animator;
    
    private static readonly int animate = Animator.StringToHash("animate");

    public int Counter
    {
        get => counter;
        set
        {
            if (value == counter)
                return;
            
            counter = value;
            text.text = counter.ToChars(3);
            animator.SetTrigger(animate);
        }
    }

    private void Start()
    {
        PhaseManager.Instance.PhaseEntered.AddListener(OnStateChanged);
    }

    private void OnStateChanged(PhaseType obj)
    {
        if (obj == PhaseType.Replenish)
        {
            Counter++;
        }
    }
}
