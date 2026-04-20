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
            Debug.Log($"🗡️ MeleeAttackStrategy.Execute() chamado em {target.name}");
            
            var targetHealth = target.GetComponent<HealthComponent>();
            if (targetHealth != null)
            {
                Debug.Log($"   ✅ HealthComponent encontrado! IsDead={targetHealth.IsDead}");
                
                if (!targetHealth.IsDead)
                {
                    targetHealth.TakeDamage(damageAmount);
                    Debug.Log($"   💥 TakeDamage({damageAmount}) chamado!");
                }
                else
                {
                    Debug.LogWarning($"   ❌ Target já está morto, dano bloqueado!");
                }
            }
            else
            {
                Debug.LogError($"   ❌ HealthComponent NÃO encontrado em {target.name}!");
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
        
        public void CachePlayerCollider(Collider2D playerCollider)
        {
            if (playerCollider != null && playerCollider.CompareTag(targetTag))
            {
                lastPlayerCollider = playerCollider;
                Debug.Log($"✅ EnemyAttackComponent: Player cacheado por trigger externo: {playerCollider.name}");
            }
        }
        
        public void ClearPlayerCollider()
        {
            Debug.Log($"🧹 EnemyAttackComponent: Limpando cache do player");
            lastPlayerCollider = null;
        }
        
        public void DoAttack(Collider2D targetCollider)
        {
            if (!CanAttack())
            {
                Debug.LogWarning($"⏸️ DoAttack bloqueado! CanAttack()=false, TimeUntilNextAttack={TimeUntilNextAttack}s");
                return;
            }

            if (attackStrategy == null)
            {
                Debug.LogError("❌ DoAttack bloqueado! attackStrategy é null!");
                return;
            }

            Debug.Log($"🗡️ DoAttack executando! Target={targetCollider.name}, Damage={damageAmount}");
            
            attackStrategy.Prepare();
            attackStrategy.Execute(targetCollider, damageAmount);
            attackStrategy.Cleanup();
            
            nextAttackTime = Time.time + attackCooldown;
            Debug.Log($"⏱️ Próximo ataque permitido em {attackCooldown}s");
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

            Debug.Log($"🎯 OnTriggerEnter2D (EnemyAttackComponent): {collision.name} entrou no range!");
            
            // Sempre cacheia o alvo para ataques via Animation Event.
            lastPlayerCollider = collision;
            Debug.Log($"✅ lastPlayerCollider cacheado: {lastPlayerCollider.name}");

            if (autoAttackOnTrigger && attackStrategy != null)
            {
                Debug.Log($"🔄 autoAttackOnTrigger={autoAttackOnTrigger}, disparando DoAttack imediato");
                DoAttack(collision);
            }
            else
            {
                Debug.Log($"⏸️ autoAttackOnTrigger={autoAttackOnTrigger} - aguardando Animation Event");
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision == lastPlayerCollider)
            {
                Debug.Log($"🚪 OnTriggerExit2D: {collision.name} saiu do range!");
                lastPlayerCollider = null;
            }
        }

        private void OnDisable()
        {
            lastPlayerCollider = null;
        }

        public void DoAttackAnimationEvent()
        {
            if (attackStrategy == null)
            {
                attackStrategy = new MeleeAttackStrategy();
            }
            
            Debug.Log($"🎬 DoAttackAnimationEvent chamado! lastPlayerCollider={lastPlayerCollider}");
            
            // Se o target foi cachieado em OnTriggerEnter2D e não saiu via OnTriggerExit2D, está no range.
            if (lastPlayerCollider != null)
            {
                Debug.Log($"✅ lastPlayerCollider encontrado: {lastPlayerCollider.name}");
                
                if (lastPlayerCollider.CompareTag(targetTag))
                {
                    Debug.Log($"✅ Target tem tag correta: {targetTag}");
                    
                    var targetHealth = lastPlayerCollider.GetComponent<HealthComponent>();
                    if (targetHealth != null)
                    {
                        Debug.Log($"✅ HealthComponent encontrado! IsDead={targetHealth.IsDead}");
                        
                        if (!targetHealth.IsDead)
                        {
                            Debug.Log($"🗡️ Aplicando dano agora! Damage={damageAmount}");
                            DoAttack(lastPlayerCollider);
                            return;
                        }
                        else
                        {
                            Debug.Log($"❌ Target já está morto!");
                        }
                    }
                    else
                    {
                        Debug.LogError($"❌ HealthComponent NÃO encontrado em {lastPlayerCollider.name}!");
                    }
                }
                else
                {
                    Debug.LogWarning($"❌ Target não tem tag '{targetTag}'!");
                }
            }
            else
            {
                Debug.LogWarning($"❌ lastPlayerCollider é null!");
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
        public string CurrentStrategyName => attackStrategy?.AttackName ?? "None";
    }
}



