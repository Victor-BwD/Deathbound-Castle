using Player;
using UnityEngine;

namespace Keeper
{
    public class KeeperAttackCollider : MonoBehaviour {
        private int damage = 2;
    
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<PlayerHealth>().PlayerTakaDamage(damage);
            }
        }
    }
}

