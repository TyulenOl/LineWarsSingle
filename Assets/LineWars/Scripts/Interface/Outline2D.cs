using UnityEngine;

namespace LineWars.Interface
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Outline2D: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool isActive;
        
        public void SetActiveOutline(bool value)
        {
            if (isActive == value) return;
            
            isActive = value;
        }
    }
}