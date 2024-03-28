using AYellowpaper.SerializedCollections;
using DataStructures;
using UnityEngine;


namespace LineWars.Controllers
{
    public class HotKeyHandler: Singleton<HotKeyHandler>
    {
        [SerializeField] private SerializedDictionary<KeyCode, HotKeyAction> codeToHotKey;
        private void Update()
        {
            if (GameRoot.Instance == null || !GameRoot.Instance.GameReady)
                return;
            
            if (SceneTransition.Loaded)
                return;
            
            foreach (var (code, action) in codeToHotKey)
            {
                if (Input.GetKeyDown(code))
                {
                    action.Invoke();
                }
            }
        }
    }
}