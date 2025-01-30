using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour{
    [SerializeField] Slider healthSlider;

    private void Awake() {
        Messenger<int, int>.AddListener(MessengerGameEvent.HEALTH_UPDATE, OnHealthUpdate);
    }

    private void OnDestroy() {
        Messenger<int, int>.RemoveListener(MessengerGameEvent.HEALTH_UPDATE, OnHealthUpdate);
    }

    private void OnHealthUpdate(int maxHealth, int newHealth){
        healthSlider.value = (float)newHealth / maxHealth;
    }
}
