using UnityEngine;

namespace Bats {
    public class BatTrigger : MonoBehaviour {
        [SerializeField] Transform[] bat;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player"))
            {
                if (ReferenceEquals(bat, null)) return;
                foreach (Transform obj in bat) {
                    obj.GetComponent<BatController>().enabled = true;
                }
            }
        }
    }
}