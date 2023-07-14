using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions;
using UnityEngine;

namespace LineWars.Model
{
    public class Graph: MonoBehaviour
    {
        public static Graph Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
        
        private IEnumerable<Node> FindNodes()
        {
            return transform.GetChildes()
                .Select(child => child.GetComponent<Node>())
                .Where(node => node != null);
        }
    }
}