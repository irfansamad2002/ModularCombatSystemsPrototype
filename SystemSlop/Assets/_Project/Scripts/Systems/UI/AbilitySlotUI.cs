using Project.Systems.Abilities.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Systems.UI
{
    public class AbilitySlotUI : MonoBehaviour
    {
        [SerializeField] private int slotIndex;
        [SerializeField] private AbilityUser abilityUser;

        [Header("UI")]
        [SerializeField] private Image cooldownOverlay;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI cooldownText;

        private void Update()
        {
            float remaining = abilityUser.GetCooldownRemaining(slotIndex);
            float max = abilityUser.GetCooldownMax(slotIndex);

            if (remaining > 0f)
            {
                float ratio = remaining / max;

                //radial fill
                cooldownOverlay.fillAmount = ratio;

                if (cooldownText != null)
                {
                    cooldownText.text = Mathf.Ceil(remaining).ToString();
                }
            }
            else
            {
                cooldownOverlay.fillAmount = 0f;

                if (cooldownText != null)
                {
                    cooldownText.text = "";
                }
            }
            if (icon != null)
            {
                icon.color = (remaining > 0f) ? new Color(1, 1, 1, 0.5f) : Color.white;
            }
        }
    }
}
