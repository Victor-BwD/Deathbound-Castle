using UnityEngine;

public class KeeperRange : MonoBehaviour {
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            transform.parent.GetComponent<Animator>().Play("KeeperAttack", -1);
        }
    }
}
