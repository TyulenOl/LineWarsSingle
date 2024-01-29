using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class BlessingsController : MonoBehaviour
    {
        [SerializeField] private int maxBlessingsCount;
        [SerializeField] private List<BlessingId> currentBlessings;

        public IReadOnlyList<BlessingId> CurrentBlessings => currentBlessings;

        public int MaxBlessingsCount => maxBlessingsCount;
    }
}