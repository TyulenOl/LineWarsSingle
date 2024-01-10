using System;
using UnityEngine;

namespace LineWars
{
    public abstract class LootedItemShower : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            animator.SetTrigger("Showing");
        }
    }
}