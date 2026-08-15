using Core.Characters;
using UnityEngine;

namespace Core.Combat
{
    public class EnemyAttackComponent : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private float attackCooldown = 0.5f;
        [SerializeField] private string targetTag = "Player";
        [SerializeField] private bool autoAttackOnTrigger = true;

        private float nextAttackTime;
        private Collider2D lastPlayerCollider;

        public void CachePlayerCollider(Collider2D playerCollider)
        {
            if (playerCollider != null && playerCollider.CompareTag(targetTag))
            {
                lastPlayerCollider = playerCollider;
            }
        }
        
        public void ClearPlayerCollider()
        {
            lastPlayerCollider = null;
        }
        
        public void DoAttack(Collider2D targetCollider)
        {
            if (!CanAttack())
            {
                return;
            }

            var targetHealth = targetCollider.GetComponent<HealthComponent>();
            if (targetHealth != null && !targetHealth.IsDead)
            {
                targetHealth.TakeDamage(damageAmount);
            }

            nextAttackTime = Time.time + attackCooldown;
        }
        
        public bool CanAttack()
        {
            return Time.time >= nextAttackTime;
        }
        
        public void ResetCooldown()
        {
            nextAttackTime = 0;
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // NOTA: Este método é chamado se EnemyAttackComponent tiver seu próprio trigger collider.
            // Em arquitetura com KeeperRange/BatRange, quem detecta é o range component.
            // Este fica como fallback apenas.
            
            if (!collision.CompareTag(targetTag))
            {
                return;
            }

            // Sempre cacheia o alvo para ataques via Animation Event.
            lastPlayerCollider = collision;

            if (autoAttackOnTrigger)
            {
                DoAttack(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision == lastPlayerCollider)
            {
                lastPlayerCollider = null;
            }
        }

        private void OnDisable()
        {
            lastPlayerCollider = null;
        }

        public void DoAttackAnimationEvent()
        {
            if (lastPlayerCollider != null)
            {
                if (lastPlayerCollider.CompareTag(targetTag))
                {
                    var targetHealth = lastPlayerCollider.GetComponent<HealthComponent>();
                    if (targetHealth != null)
                    {
                        if (!targetHealth.IsDead)
                        {
                            DoAttack(lastPlayerCollider);
                            return;
                        }
                    }
                }
            }

            // Se o alvo saiu do range ou morreu, limpa o cache.
            lastPlayerCollider = null;
        }


        /// <summary>
        /// Getters públicos para debug/UI
        /// </summary>
        public int DamageAmount => damageAmount;
        public float AttackCooldown => attackCooldown;
        public float TimeUntilNextAttack => Mathf.Max(0, nextAttackTime - Time.time);
    }
}



