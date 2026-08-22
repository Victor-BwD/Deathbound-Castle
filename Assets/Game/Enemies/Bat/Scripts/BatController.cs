using Core.Characters;
using Core.Combat;
using Player;
using UnityEngine;

namespace Bats
{
    [RequireComponent(typeof(EnemyAttackComponent))]
    public class BatController : EnemyCharacter
    {
        [SerializeField] public Transform player;
        [SerializeField] private float chaseSpeed = 2f;
        [SerializeField] private float attackRange = 0.8f;

        public BatTrigger batTrigger;

        private CircleCollider2D circleCollider2D;
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

        protected override void Awake()
        {
            base.Awake();

            // O bat começa desabilitado até o BatTrigger ativá-lo, mas o listener de
            // OnDeath (registrado no Awake da base) roda de qualquer forma. Por isso
            // essas referências precisam existir aqui, não no Start — senão um bat que
            // morre antes de ser ativado estoura NullReferenceException em OnDeath.
            circleCollider2D = GetComponent<CircleCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            if (attackComponent == null)
            {
                attackComponent = GetComponent<EnemyAttackComponent>();
            }
        }

        void Start()
        {
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
        }

        void FixedUpdate()
        {
            if (Health.IsDead)
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

            float distance = Vector2.Distance(rb.position, playerCapsule.bounds.center);

            if (distance > attackRange)
            {
                // Chase
                rb.MovePosition(Vector2.MoveTowards(
                    rb.position,
                    playerCapsule.bounds.center,
                    chaseSpeed * Time.fixedDeltaTime
                ));
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

        protected override void OnDeath()
        {
            circleCollider2D.enabled = false;
            rb.gravityScale = 1;
            this.enabled = false;

            if (batTrigger != null)
            {
                batTrigger.RemoveGameObject(this.gameObject.transform);
            }

            base.OnDeath();
        }
    }
}
