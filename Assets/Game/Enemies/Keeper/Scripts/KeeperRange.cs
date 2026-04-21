using Core.Combat;
using UnityEngine;

namespace Keeper
{
    public class KeeperRange : MonoBehaviour {
        private bool isPlayerInRange;
        private EnemyAttackComponent attackComponent;

        public bool IsPlayerInRange => isPlayerInRange;

        private void Awake()
        {
            attackComponent = GetComponentInParent<EnemyAttackComponent>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player")) {
                Debug.Log($"KeeperRange: Player entrou! Notificando EnemyAttackComponent...");
                isPlayerInRange = true;
                
                if (attackComponent != null)
                {
                    attackComponent.CachePlayerCollider(collision);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.CompareTag("Player")) {
                Debug.Log($"KeeperRange: Player saiu!");
                isPlayerInRange = false;
                
                if (attackComponent != null)
                {
                    attackComponent.ClearPlayerCollider();
                }
            }
        }
    }
}

