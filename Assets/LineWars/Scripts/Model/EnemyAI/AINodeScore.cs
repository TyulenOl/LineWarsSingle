using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class AINodeScore : MonoBehaviour
    {
        [field: SerializeField, Min(0)] private int score;

        public int Score => score;
    }
}
