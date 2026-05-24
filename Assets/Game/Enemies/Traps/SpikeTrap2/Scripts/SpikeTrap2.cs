using Core.Characters;
using Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace Traps
{
    public class SpikeTrap2 : MonoBehaviour
    {
        private HealthComponent playerHealth;
        [SerializeField] private float damageCooldown = 0.5f;
        private float lastDamageTime = -1f;
        private int lastDamageFrame = -1;


        // Start is called before the first frame update
        void Start()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerHealth = playerObj.GetComponent<HealthComponent>();
                if (playerHealth == null)
                {
                    Debug.LogError("SpikeTrap2: Player não tem HealthComponent!");
                }
            }
            else
            {
                Debug.LogError("SpikeTrap2: Player não encontrado!");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!CanDamageNow())
                {
                    return;
                }

                if (playerHealth != null)
                {
                    lastDamageTime = Time.time;
                    lastDamageFrame = Time.frameCount;
                    playerHealth.TakeDamage(1);

                    if (playerHealth.IsDead)
                    {
                        GetComponent<BoxCollider2D>().enabled = false;
                    }
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
