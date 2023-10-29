using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(fileName = "New SFX List", menuName = "Audio/SFX List")]
    public class SFXList : ScriptableObject, IEnumerable<SFXData>
    {
        [SerializeField] private List<SFXData> data;


        public IEnumerator<SFXData> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}