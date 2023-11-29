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
        [field: SerializeField] public SpriteRenderer imagePrefab;
        [field: SerializeField] public SpriteRenderer buyOnNodeInfo;

        public void ReDrawBuyInfo(bool isActive)
        {
            buyOnNodeInfo.gameObject.SetActive(isActive);
        }
        
        public void ReDrawCommads(IEnumerable<CommandType> commandTypes)
        {
            Clean();
            
            var types = commandTypes.Distinct().ToList();
            if(types.Count == 1 && types[0] == CommandType.None)
                return;
            
            foreach (var commandType in types)
            {
                var instance = Instantiate(imagePrefab, ImagesLayout.transform);
                instance.sprite = DrawHelper.GetSpriteByCommandType(commandType);
            }
        }

        public override void ReDrawCommads(CommandType commandType)
        {
             ReDrawCommads(new []{commandType});
        }

        private void Clean()
        {
            var images = ImagesLayout.GetComponentsInChildren<SpriteRenderer>();
            foreach (var image in images)
            {
                Destroy(image.gameObject);
            }
        }
    }
}