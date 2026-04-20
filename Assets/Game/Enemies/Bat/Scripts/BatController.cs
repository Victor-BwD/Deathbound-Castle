using Core.Characters;
using Player;
using UnityEngine;

namespace Bats
{
    public class BatController : MonoBehaviour
    {
        [SerializeField] public Transform player;
        [SerializeField] private float attackTime;
    
        private HealthComponent healthComponent;
        private Collider2D circleCollider2D;
        private Rigidbody2D rb;
        private int damage = 1;
    
        // Start is called before the first frame update
        void Start()
        {
            attackTime = 0;
            healthComponent = GetComponent<HealthComponent>();
            circleCollider2D = GetComponent<CircleCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            
            if (healthComponent != null)
            {
                healthComponent.OnDeath.AddListener(HandleDeath);
            }
        }
    
        // Update is called once per frame
        void Update()
        {
            if (healthComponent != null && healthComponent.IsDead)
            {
                return;
            }
    
            if (Vector2.Distance(transform.position, player.GetComponent<CapsuleCollider2D>().bounds.center) > 0.8f)
            {
                attackTime = 0;
                transform.position = Vector2.MoveTowards(transform.position,
                    player.GetComponent<CapsuleCollider2D>().bounds.center, 2f * Time.deltaTime);
            }
            else
            {
                attackTime += Time.deltaTime;
                if (attackTime >= 0.5)
                {
                    attackTime = 0;
                    player.GetComponent<PlayerHealth>().PlayerTakaDamage(damage);
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
