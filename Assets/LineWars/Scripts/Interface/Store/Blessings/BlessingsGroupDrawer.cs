using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingsGroupDrawer : MonoBehaviour
    {
        [SerializeField] private BlessingDraggableSet blessingDraggableSetPrefab;
        [SerializeField] private LayoutGroup layoutToGenerateBlessings;
        [SerializeField] private TMP_Text groupName;
        
        public void Init(IEnumerable<BlessingId> blessings)
        {
            groupName.text = DrawHelper.GetBlessingNameByBlessingID(blessings.First());
            foreach (var blessing in blessings)
            {
                var instance = Instantiate(blessingDraggableSetPrefab, layoutToGenerateBlessings.transform);
                instance.Init(blessing);
            }
        }
    }
}
