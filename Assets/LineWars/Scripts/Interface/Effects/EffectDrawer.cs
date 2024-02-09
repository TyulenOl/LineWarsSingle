using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class EffectDrawer : MonoBehaviour
    {
        [SerializeField] private GameObject powerObjects;
        [SerializeField] private TextMeshProUGUI powerText;
        [SerializeField] private GameObject roundsObjects;
        [SerializeField] private TextMeshProUGUI roundText;
        [SerializeField] private Image image;

        public void DrawEffect(Effect<Node, Edge, Unit> effect)
        {
            if (effect is IPowerEffect powerEffect)
            {
                powerObjects.SetActive(true);
                powerText.text = powerEffect.Power.ToString();
            }
            else
            {
                powerObjects.SetActive(false);
            }

            if (effect is ITemporaryEffect temporaryEffect)
            {
                roundsObjects.SetActive(true);
                roundText.text = temporaryEffect.Rounds.ToString();
            }
            else
            {
                roundsObjects.SetActive(false);
            }

            if (GameRoot.Instance.DrawHelper.EffectTypeToSprite.ContainsKey(effect.EffectType))
            {
                var icon = GameRoot.Instance.DrawHelper.EffectTypeToSprite[effect.EffectType];
                image.sprite = icon;
            }
        }
    }
}
