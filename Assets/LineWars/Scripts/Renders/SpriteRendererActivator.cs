using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererActivator : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
