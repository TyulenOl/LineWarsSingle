using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Interface
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(LineDrawer))]
    public class ColliderAlligner : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private BoxCollider2D boxCollider;
        private LineDrawer lineDrawer;

        private void Awake() 
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
            lineDrawer = GetComponent<LineDrawer>();
        }

        private void OnEnable() 
        {
            lineDrawer.LineDrawn.AddListener(OnAllignCollider);
        }

        private void OnDisable() 
        {
            lineDrawer.LineDrawn.RemoveListener(OnAllignCollider);
        }

        private void OnAllignCollider()
        {
            boxCollider.size = spriteRenderer.size;
        }
        
    }
}

