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
        private Characters characters;
        private Animator receiveSkinAnimator;
        private KeeperSounds keeperSounds;
        private Transform playerTransform;

        void Start() {
            collider2D = GetComponent<Collider2D>();
            circleCollider = GetComponentInChildren<CircleCollider2D>();
            characters = GetComponent<Characters>();
            receiveSkinAnimator = skin.GetComponent<Animator>();
            keeperSounds = GetComponentInChildren<KeeperSounds>();
            
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        void FixedUpdate() {
            if(characters.life <= 0) {
                keeperSounds.DieSound();
                collider2D.enabled = false;
                circleCollider.enabled = false;
                this.enabled = false;
                return;
            }
    
            if (receiveSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("KeeperAttack")) {
                return;
            }

            Patrol();
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

