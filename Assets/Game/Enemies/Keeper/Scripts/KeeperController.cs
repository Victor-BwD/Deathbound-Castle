using Core.Characters;
using Core.Combat;
using UnityEngine;

namespace Keeper
{
    public class KeeperController : EnemyCharacter, IAttackable {

        [SerializeField] private Transform a_point, b_point;
        [SerializeField] private Transform keeperRange;
        [SerializeField] private float speedPatrol = 2.2f;
        private bool goRight;
        private Collider2D circleCollider;
        private Collider2D collider2D;
        [SerializeField] private EnemyAttackComponent attackComponent;
        private Animator receiveSkinAnimator;
        private KeeperSounds keeperSounds;
        private Transform playerTransform;
        private KeeperRange keeperRangeComponent;

        private void OnValidate()
        {
            if (attackComponent == null)
            {
                attackComponent = GetComponentInChildren<EnemyAttackComponent>();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            attackComponent = GetComponentInChildren<EnemyAttackComponent>();

            keeperSounds = GetComponentInChildren<KeeperSounds>();
            if (keeperSounds == null)
            {
                Debug.LogWarning("KeeperSounds não encontrado no Keeper.");
            }
        }

        void Start() {
            collider2D = GetComponent<Collider2D>();
            circleCollider = GetComponentInChildren<CircleCollider2D>();
            receiveSkinAnimator = Skin.GetComponent<Animator>();
            keeperRangeComponent = keeperRange.GetComponent<KeeperRange>();

            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        void FixedUpdate() {
            if (Health.IsDead) {
                return;
            }
    
            if (receiveSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("KeeperAttack")) {
                return;
            }

            // Checar se player está no range e atacar em loop
            if (keeperRangeComponent != null && keeperRangeComponent.IsPlayerInRange)
            {
                if (attackComponent != null && attackComponent.CanAttack())
                {
                    receiveSkinAnimator.Play("KeeperAttack", -1);
                }
                return;
            }

            Patrol();
        }

        protected override void OnDeath()
        {
            if (keeperSounds != null)
            {
                keeperSounds.DieSound();
            }

            collider2D.enabled = false;
            circleCollider.enabled = false;
            this.enabled = false;

            base.OnDeath();
        }

        private void Patrol()
        {
            var absScaleX = Mathf.Abs(Skin.localScale.x);
            Skin.localScale = new Vector3(goRight ? absScaleX : -absScaleX, Skin.localScale.y, Skin.localScale.z);

            transform.position = PatrolMath.Step(transform.position, a_point.position, b_point.position, speedPatrol, ref goRight, out _);
        }

        public void OnPlayerAttack(Vector3 attackerPosition)
        {
            float directionToPlayer = attackerPosition.x - transform.position.x;
            
            if (directionToPlayer > 0)
            {
                goRight = (b_point.position.x > a_point.position.x);
            }
            else if (directionToPlayer < 0)
            {
                goRight = (b_point.position.x < a_point.position.x);
            }
        }
    }
}
