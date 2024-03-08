using System;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    [RequireComponent(typeof(Animator))]
    public class TextAnimator: MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private Animator animator;
        private static readonly int animate = Animator.StringToHash("animate");
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetText(string text)
        {
            this.text.text = text;
            animator.SetTrigger(animate);
        }
        
        public void SetTextImmediate(string text)
        {
            this.text.text = text;
        }
    }
}