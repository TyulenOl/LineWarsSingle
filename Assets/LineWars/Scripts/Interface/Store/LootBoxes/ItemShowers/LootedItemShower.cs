using System;
using UnityEngine;

namespace LineWars
{
    public abstract class LootedItemShower : MonoBehaviour
    {
        private Animator animator;
        private static readonly int showing = Animator.StringToHash("Showing");

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            animator.SetTrigger(showing);
        }
    }
}