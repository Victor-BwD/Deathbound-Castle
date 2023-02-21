using System.Collections.Generic;
using UnityEngine;

namespace Bats {
    public class BatTrigger : MonoBehaviour
    {
        [SerializeField] private List<Transform> bats;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player"))
            {
                foreach (Transform obj in bats) {
                    obj.GetComponent<BatController>().enabled = true;
                }
            }
        }
        public void RemoveGameObject(Transform go) {
            bats.Remove(go);
        }
    }
    
    
}