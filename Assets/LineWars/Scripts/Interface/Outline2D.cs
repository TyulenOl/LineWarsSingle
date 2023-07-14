using UnityEngine;

namespace LineWars.Interface
{
    public class Outline2D: MonoBehaviour
    {
        [SerializeField] private Material withOutline;
        [SerializeField] private Material withoutOutline;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool isActive;
        
        public void SetActiveOutline(bool value)
        {
            if (isActive == value) return;
            
            if (value)
                spriteRenderer.sharedMaterial = withOutline;
            else
                spriteRenderer.sharedMaterial = withoutOutline;

            isActive = value;
        }
    }
}