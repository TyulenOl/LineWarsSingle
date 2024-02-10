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

        private Effect<Node, Edge, Unit> effect;

        public void DrawEffect(Effect<Node, Edge, Unit> effect)
        {
            this.effect = effect;
            if (effect is IPowerEffect powerEffect)
            {
                powerObjects.SetActive(true);
                powerText.text = powerEffect.Power.ToString();
                powerEffect.PowerChanged += OnPowerChanged;
            }
            else
            {
                powerObjects.SetActive(false);
            }

            if (effect is ITemporaryEffect temporaryEffect)
            {
                roundsObjects.SetActive(true);
                roundText.text = temporaryEffect.Rounds.ToString();
                temporaryEffect.RoundsChanged += OnRoundsChanged;
            }
            else
            {
                roundsObjects.SetActive(false);
            }

            if (GameRoot.Instance != null &&
                GameRoot.Instance.DrawHelper != null &&
                GameRoot.Instance.DrawHelper.EffectTypeToSprite.ContainsKey(effect.EffectType))
            {
                var icon = GameRoot.Instance.DrawHelper.EffectTypeToSprite[effect.EffectType];
                image.sprite = icon;
            }
        }

        private void OnDestroy()
        {
            if (effect is IPowerEffect powerEffect)
            {
                powerEffect.PowerChanged -= OnPowerChanged;
            }

            if (effect is ITemporaryEffect temporaryEffect)
            {
                temporaryEffect.RoundsChanged -= OnRoundsChanged;
            }
        }

        private void OnRoundsChanged(ITemporaryEffect effect, int prevRounds, int currentRounds)
        {
            roundText.text = effect.Rounds.ToString();
        }

        private void OnPowerChanged(IPowerEffect effect, int prevRounds, int currentRounds)
        {
            roundText.text = effect.Power.ToString();
        }
    }
}
