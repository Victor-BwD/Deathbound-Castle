using Core.Characters;
using Core.Combat;
using Player;
using UnityEngine;

namespace Bats
{
    public class BatController : MonoBehaviour
    {
        [SerializeField] public Transform player;
        [SerializeField] private float chaseSpeed = 2f;
        [SerializeField] private float attackRange = 0.8f;
    
        private HealthComponent healthComponent;
        private Collider2D circleCollider2D;
        private Rigidbody2D rb;
    
        void Start()
        {
            healthComponent = GetComponent<HealthComponent>();
            circleCollider2D = GetComponent<CircleCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            
            
            if (healthComponent != null)
            {
                healthComponent.OnDeath.AddListener(HandleDeath);
            }
        }
    
        void Update()
        {
            if (healthComponent != null && healthComponent.IsDead)
            {
                return;
            }
    
            if (player == null)
            {
                return;
            }

            float distance = Vector2.Distance(transform.position, player.GetComponent<CapsuleCollider2D>().bounds.center);
            
            if (distance > attackRange)
            {
                // Chase
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    player.GetComponent<CapsuleCollider2D>().bounds.center,
                    chaseSpeed * Time.deltaTime
                );
            }
        }

        private void HandleDeath()
        {
            circleCollider2D.enabled = false;
            rb.gravityScale = 1;
            this.enabled = false;
            Destroy(gameObject, 2);
            BatTrigger batTrigger = FindObjectOfType<BatTrigger>();
            if (batTrigger != null)
            {
                batTrigger.RemoveGameObject(this.gameObject.transform);
            }
        }
    }
}
