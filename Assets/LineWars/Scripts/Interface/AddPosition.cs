using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPosition : MonoBehaviour
{
    [SerializeField] private Transform value;

    private void Start()
    {
        if (value != null)
        {
            transform.position += value.position;
        }
    }
}