using UnityEngine;

public class KeeperAttackCollider : MonoBehaviour
{
    private int damage = 2;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Characters>().PlayerTakaDamage(damage);
        }
    }
}
