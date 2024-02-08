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
        [SerializeField] private EffectIcons effectIcons;
        [SerializeField] private Image image;

        public void DrawEffect(Effect<Node, Edge, Unit> effect)
        {
            if (effect is IPowerEffect powerEffect)
            {
                powerObjects.SetActive(true);
                powerText.text = powerEffect.Power.ToString();
            }

            if (effect is ITemporaryEffect temporaryEffect)
            {
                roundsObjects.SetActive(true);
                roundText.text = temporaryEffect.Rounds.ToString();
            }

            var icon = effectIcons[effect.EffectType];
            image.sprite = icon;

        }
    }
}
