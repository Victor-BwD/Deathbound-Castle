using Core.Characters;
using UnityEngine;

namespace Core.Combat
{
    public interface IAttackStrategy
    {
        void Execute(Collider2D target, int damageAmount);
        void Prepare();
        void Cleanup();
        string AttackName { get; }
    }
    
    public class MeleeAttackStrategy : IAttackStrategy
    {
        public string AttackName => "Melee";

        public void Execute(Collider2D target, int damageAmount)
        {
            var targetHealth = target.GetComponent<HealthComponent>();
            if (targetHealth != null && !targetHealth.IsDead)
            {
                targetHealth.TakeDamage(damageAmount);
            }
        }

        public void Prepare()
        {
            // Pode adicionar animação, som, efeitos aqui
        }

        public void Cleanup()
        {
            // Cleanup após ataque
        }
    }
    
    public class EnemyAttackComponent : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private float attackCooldown = 0.5f;
        [SerializeField] private string targetTag = "Player";
        [SerializeField] private bool autoAttackOnTrigger = true;
        
        private IAttackStrategy attackStrategy;
        private float nextAttackTime;
        private Collider2D lastPlayerCollider;
        
        public void SetAttackStrategy(IAttackStrategy strategy)
        {
            attackStrategy = strategy;
        }
        
        public void DoAttack(Collider2D targetCollider)
        {
            if (!CanAttack() || attackStrategy == null)
            {
                return;
            }

            attackStrategy.Prepare();
            attackStrategy.Execute(targetCollider, damageAmount);
            attackStrategy.Cleanup();
            
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
            if (!autoAttackOnTrigger || attackStrategy == null)
            {
                return;
            }

            if (collision.CompareTag(targetTag))
            {
                lastPlayerCollider = collision;  // ← CACHEIA!
                DoAttack(collision);
            }
        }

        public void DoAttackAnimationEvent()
        {
            if (attackStrategy == null)
            {
                attackStrategy = new MeleeAttackStrategy();
            }
            
            if (lastPlayerCollider != null && lastPlayerCollider.CompareTag(targetTag))
            {
                DoAttack(lastPlayerCollider);
                return;
            }
            
            var playerObj = GameObject.FindWithTag(targetTag);
            if (playerObj != null)
            {
                var playerCollider = playerObj.GetComponent<Collider2D>();
                if (playerCollider != null)
                {
                    DoAttack(playerCollider);
                    lastPlayerCollider = playerCollider;
                    return;
                }
            }
        }

        /// <summary>
        /// Getters públicos para debug/UI
        /// </summary>
        public int DamageAmount => damageAmount;
        public float AttackCooldown => attackCooldown;
        public float TimeUntilNextAttack => Mathf.Max(0, nextAttackTime - Time.time);
        public string CurrentStrategyName => attackStrategy?.AttackName ?? "None";
    }
}



