using UnityEngine;
using Core.Characters;
using Core.Services;

namespace Features.Enemies.Bat
{
    /// <summary>
    /// Versão refatorada do BatController usando HealthComponent
    /// </summary>
    public class BatAI : MonoBehaviour
    {
        [SerializeField] private Transform targetPlayer;
        [SerializeField] private float chaseSpeed = 2f;
        [SerializeField] private float attackCooldown = 0.5f;
        [SerializeField] private int damageAmount = 1;

        private Characters batCharacter;
        private Rigidbody2D rb;
        private float nextAttackTime;

        private void Start()
        {
            batCharacter = GetComponent<Characters>();
            rb = GetComponent<Rigidbody2D>();

            if (targetPlayer == null)
            {
                var playerObj = GameObject.FindGameObjectWithTag("Player");
                targetPlayer = playerObj?.transform;
            }

            // NOVO: Subscribe a eventos de morte
            batCharacter.Health.OnDeath.AddListener(() => {
                rb.gravityScale = 1;
                GetComponent<Collider2D>().enabled = false;
                enabled = false;
                Destroy(gameObject, 2f);
            });
        }

        private void Update()
        {
            if (batCharacter.Health.IsDead) return;

            if (targetPlayer != null)
            {
                // Chase player
                float distance = Vector2.Distance(transform.position, targetPlayer.position);
                
                if (distance > 0.8f)
                {
                    // Move towards
                    transform.position = Vector2.MoveTowards(
                        transform.position,
                        targetPlayer.position,
                        chaseSpeed * Time.deltaTime
                    );
                    nextAttackTime = 0;
                }
                else
                {
                    // Attack cooldown
                    if (Time.time >= nextAttackTime)
                    {
                        Attack();
                        nextAttackTime = Time.time + attackCooldown;
                    }
                }
            }
        }

        private void Attack()
        {
            var targetHealth = targetPlayer.GetComponent<HealthComponent>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damageAmount);
            }
        }
    }
}