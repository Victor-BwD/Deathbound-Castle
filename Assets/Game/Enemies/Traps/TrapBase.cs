using Core.Characters;
using UnityEngine;

namespace Traps
{
    /// <summary>
    /// Dano por contato com o player, com cooldown. Subclasses só ajustam o
    /// knockback (default: nenhum) sobrescrevendo KnockbackForce.
    /// </summary>
    public abstract class TrapBase : MonoBehaviour
    {
        [SerializeField] private float damageCooldown = 0.5f;

        private HealthComponent playerHealth;
        private Rigidbody2D playerRb;
        private float lastDamageTime = -1f;
        private int lastDamageFrame = -1;

        protected virtual Vector2 KnockbackForce => Vector2.zero;

        protected virtual void Start()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerHealth = playerObj.GetComponent<HealthComponent>();
                playerRb = playerObj.GetComponent<Rigidbody2D>();

                if (playerHealth == null)
                {
                    Debug.LogError($"{GetType().Name}: Player não tem HealthComponent!");
                }
            }
            else
            {
                Debug.LogError($"{GetType().Name}: Player não encontrado!");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player") || playerHealth == null || !CanDamageNow())
            {
                return;
            }

            lastDamageTime = Time.time;
            lastDamageFrame = Time.frameCount;

            if (KnockbackForce != Vector2.zero && playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                playerRb.AddForce(KnockbackForce);
            }

            playerHealth.TakeDamage(1);

            if (playerHealth.IsDead)
            {
                var boxCollider = GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    boxCollider.enabled = false;
                }
            }
        }

        private bool CanDamageNow()
        {
            if (Time.frameCount == lastDamageFrame)
            {
                return false;
            }

            return Time.time >= lastDamageTime + damageCooldown;
        }
    }
}
