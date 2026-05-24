using Core.Characters;
using Core.Combat;
using Player;
using UnityEngine;

namespace Bats
{
    [RequireComponent(typeof(EnemyAttackComponent))]
    public class BatController : MonoBehaviour
    {
        [SerializeField] public Transform player;
        [SerializeField] private float chaseSpeed = 2f;
        [SerializeField] private float attackRange = 0.8f;
    
        private HealthComponent healthComponent;
        private Collider2D circleCollider2D;
        private Rigidbody2D rb;
        [SerializeField] private EnemyAttackComponent attackComponent;
        private CapsuleCollider2D playerCapsule;
        private Collider2D playerCollider;

        private void OnValidate()
        {
            if (attackComponent == null)
            {
                attackComponent = GetComponent<EnemyAttackComponent>();
            }
        }
    
        void Start()
        {
            healthComponent = GetComponent<HealthComponent>();
            circleCollider2D = GetComponent<CircleCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            attackComponent = GetComponent<EnemyAttackComponent>();

            if (attackComponent != null)
            {
                attackComponent.SetAttackStrategy(new MeleeAttackStrategy());
            }
            else
            {
                Debug.LogWarning("BatController: EnemyAttackComponent não encontrado no mesmo GameObject.");
            }

            if (player == null)
            {
                var playerObj = GameObject.FindWithTag("Player");
                if (playerObj != null)
                {
                    player = playerObj.transform;
                }
            }

            if (player != null)
            {
                playerCapsule = player.GetComponent<CapsuleCollider2D>();
                playerCollider = player.GetComponent<Collider2D>();
            }
            
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

            if (playerCapsule == null)
            {
                playerCapsule = player.GetComponent<CapsuleCollider2D>();
            }

            if (playerCapsule == null)
            {
                return;
            }

            float distance = Vector2.Distance(transform.position, playerCapsule.bounds.center);
            
            if (distance > attackRange)
            {
                // Chase
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    playerCapsule.bounds.center,
                    chaseSpeed * Time.deltaTime
                );
            }
            else
            {
                TryAttackPlayer();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
            {
                return;
            }

            playerCollider = collision;
            TryAttackPlayer();
        }

        private void TryAttackPlayer()
        {
            if (attackComponent != null && attackComponent.CanAttack())
            {
                if (playerCollider == null && player != null)
                {
                    playerCollider = player.GetComponent<Collider2D>();
                }

                if (playerCollider != null)
                {
                    attackComponent.DoAttack(playerCollider);
                }
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
