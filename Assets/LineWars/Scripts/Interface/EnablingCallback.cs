using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnablingCallback : MonoBehaviour
{
    public UnityEvent enabled;
    public UnityEvent disabled;

    private void OnEnable()
    {
        enabled.Invoke();
    }

    private void OnDisable()
    {
        disabled.Invoke();
    }
}
