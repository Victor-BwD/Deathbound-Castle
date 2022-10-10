using UnityEngine;

public class BatController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float attackTime;

    private Characters charactersController;

    private Collider2D circleCollider2D;

    private Rigidbody2D rb;

    private int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        attackTime = 0;
        charactersController = GetComponent<Characters>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (charactersController.life <= 0)
        {
            circleCollider2D.enabled = false;
            rb.gravityScale = 1;
            this.enabled = false;

            Destroy(gameObject, 2);
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
}