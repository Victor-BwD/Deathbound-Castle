using Core.Characters;
using UnityEngine;

namespace Player {
    public class AddHealth : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D col) {
            if (!col.CompareTag("Player"))
            {
                return;
            }

            var health = col.GetComponent<HealthComponent>();
            if (health != null && health.CurrentHealth < health.MaxHealth)
            {
                health.Heal(1);
                Destroy(this.gameObject);
            }
        }
    }
}
