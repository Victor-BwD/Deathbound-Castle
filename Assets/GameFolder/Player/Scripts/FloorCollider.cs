using UnityEngine;

namespace Player {
    public class FloorCollider : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip groundedSound;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Floor"))
            {
                audioSource.PlayOneShot(groundedSound, 0.1f);
            }
        }
    }
}

