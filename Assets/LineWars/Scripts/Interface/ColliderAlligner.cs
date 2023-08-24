using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Interface
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ColliderAlligner : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private BoxCollider2D boxCollider;

        private void Awake() 
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            AllignCollider();
        }

        private void AllignCollider()
        {
            boxCollider.size = spriteRenderer.size;
        }
        
    }
}

