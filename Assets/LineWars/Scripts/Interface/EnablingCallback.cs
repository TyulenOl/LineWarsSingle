using System;
using UnityEngine;
using UnityEngine.Events;

public class EnablingCallback : MonoBehaviour
{
    [field: SerializeField] public UnityEvent Enabled { get; private set; }
    [field: SerializeField] public UnityEvent Disabled { get; private set; }

    private void OnEnable()
    {
        Enabled?.Invoke();
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        Disabled?.Invoke();
#else
        try
        {
            Disabled?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
#endif
    }
}