using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroRotationScript : MonoBehaviour
{
    private void OnEnable()
    {
        var parentRotation = GetComponentInParent<Transform>().rotation;
        transform.rotation = new Quaternion(parentRotation.x, parentRotation.y, -parentRotation.z, parentRotation.w);
    }
}
