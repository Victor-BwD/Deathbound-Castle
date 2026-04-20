using Core.Characters;
using Core.Combat;
using Core.Services;
using UnityEngine;

namespace Keeper
{
    public class KeeperController : MonoBehaviour, IAttackable {
        
        [SerializeField] private Transform a_point, b_point;
        [SerializeField] private Transform skin;
        [SerializeField] private Transform keeperRange;
        [SerializeField] private float speedPatrol = 2.2f;
        
        private bool goRight;
        private Collider2D circleCollider;
        private Collider2D collider2D;
        private HealthComponent healthComponent;
        private EnemyAttackComponent attackComponent;
        private Animator receiveSkinAnimator;
        private KeeperSounds keeperSounds;
        private Transform playerTransform;
        
        private void Awake()
        {
            attackComponent = GetComponentInChildren<EnemyAttackComponent>();
            attackComponent.SetAttackStrategy(new MeleeAttackStrategy());
            
            if (ServiceLocator.TryGet<KeeperSounds>(out var sounds))
            {
                keeperSounds = sounds;
            }
            else
            {
                Debug.LogWarning("KeeperSounds não registrado no ServiceLocator!");
                keeperSounds = GetComponentInChildren<KeeperSounds>();
            }
        }

        void Start() {
            collider2D = GetComponent<Collider2D>();
            circleCollider = GetComponentInChildren<CircleCollider2D>();
            healthComponent = GetComponent<HealthComponent>();
            receiveSkinAnimator = skin.GetComponent<Animator>();


            
            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            
            if (healthComponent != null)
            {
                healthComponent.OnDeath.AddListener(HandleDeath);
            }
        }

        void FixedUpdate() {
            if(healthComponent != null && healthComponent.IsDead) {
                return;
            }
    
            if (receiveSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("KeeperAttack")) {
                return;
            }

            Patrol();
        }

        private void HandleDeath()
        {
            keeperSounds.DieSound();
            collider2D.enabled = false;
            circleCollider.enabled = false;
            this.enabled = false;
        }

        private void Patrol()
        {
            if (goRight) {
                skin.localScale = new Vector3(Mathf.Abs(skin.localScale.x), skin.localScale.y, skin.localScale.z);

                if (Vector2.Distance(transform.position, b_point.position) < 0.1f) {
                    goRight = false;
                }
    
                transform.position = Vector3.MoveTowards(transform.position, b_point.position, speedPatrol * Time.deltaTime);
            }
            else {
                skin.localScale = new Vector3(-Mathf.Abs(skin.localScale.x), skin.localScale.y, skin.localScale.z);

                if (Vector2.Distance(transform.position, a_point.position) < 0.1f)
                {
                    goRight = true;
                }
                transform.position = Vector3.MoveTowards(transform.position, a_point.position, speedPatrol * Time.deltaTime);
            }
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

