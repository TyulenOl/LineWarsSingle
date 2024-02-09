using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    
    public class NodeTargetDrawer : TargetDrawer
    {
        [field: SerializeField] public LayoutGroup ImagesLayout { get; set; }
        [field: SerializeField] public Image OrderImagePrefab { get; set; }
        [field: SerializeField] public SpriteRenderer BuyOnNodeInfo { get; set; }

        private List<Image> orderImages = new();

        public void ReDrawBuyInfo(bool isActive)
        {
            BuyOnNodeInfo.gameObject.SetActive(isActive);
        }
        
        public void ReDrawCommads(IEnumerable<CommandType> commandTypes)
        {
            Clean();
            
            var types = commandTypes
                .Distinct()
                .Where(x => x != CommandType.None)
                .OrderBy(x => x)
                .ToList();
            
            foreach (var commandType in types)
            {
                var instance = Instantiate(OrderImagePrefab, ImagesLayout.transform);
                instance.sprite = DrawHelper.GetOrderIconByCommandType(commandType);
                orderImages.Add(instance);
            }
        }

        public override void ReDrawCommads(CommandType commandType)
        {
             ReDrawCommads(new []{commandType});
        }

        private void Clean()
        {
            foreach (var el in orderImages)
                Destroy(el.gameObject);
            orderImages.Clear();
        }
    }
}