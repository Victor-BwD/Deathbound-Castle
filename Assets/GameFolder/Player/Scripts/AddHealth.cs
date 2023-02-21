using UnityEngine;

namespace Player {
    public class AddHealth : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Player") && col.GetComponent<Characters>().life < 5) {
                col.GetComponent<Characters>().life++;
                Destroy(this.gameObject);
            }
        }
    }
}

