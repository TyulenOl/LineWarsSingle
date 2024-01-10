using System;
using UnityEngine;

namespace LineWars
{
    public abstract class LootedItemShower : MonoBehaviour
    {
        private Animator animator;

        private void OnEnable()
        {
            animator.SetTrigger("Showing");
        }
    }
}