using UnityEngine;

public class KeeperAttackCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Characters>().life--;
        }
    }
}
