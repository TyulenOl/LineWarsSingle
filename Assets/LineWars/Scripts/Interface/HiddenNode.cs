using UnityEngine;

namespace LineWars.Interface
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HiddenNode: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer hiddenNodeRenderer;
        
        
        public void SetActive(bool value)
        {
            hiddenNodeRenderer.enabled = value;
        }
    }
}