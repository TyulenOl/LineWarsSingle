using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnablingCallback : MonoBehaviour
{
    [field: SerializeField] public UnityEvent Enabled { get; private set; }
    [field: SerializeField] public UnityEvent Disabled { get; private set; }

    private void OnEnable()
    {
        Enabled.Invoke();
    }

    private void OnDisable()
    {
        Disabled.Invoke();
    }
}