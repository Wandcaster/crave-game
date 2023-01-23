using ElRaccoone.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class HealthBar : MonoBehaviour {
        [SerializeField] private Slider slider;

        [SerializeField] private Image bar;

        public void SetHp(float current, float max) {
            slider.maxValue = max;
            gameObject.TweenValueFloat(current, 0.3f, p => slider.value = p).SetFrom(slider.value);
            var percent = current / max;
            bar.color = percent switch {
                > 0.7f => Color.green,
                > 0.3f => Color.yellow,
                _ => Color.red
            };
        }
    }
}
