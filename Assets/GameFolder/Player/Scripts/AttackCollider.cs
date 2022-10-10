using UnityEngine;

public class AttackCollider : MonoBehaviour {
    public Transform player;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            if(player.GetComponent<PlayerController>().comboNumber == 1) {
                collision.GetComponent<Characters>().life--;
            }
            if(player.GetComponent<PlayerController>().comboNumber == 2) {
                collision.GetComponent<Characters>().life -= 2;
            }
        }
    }
}
