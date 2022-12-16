using Player;
using UnityEngine;


namespace Traps
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BearTrap : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform skin;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip bearTrapAudio;
        private PlayerHealth playerHealth;
    
    
        void Start()
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")){
                _audioSource.PlayOneShot(bearTrapAudio);
                playerHealth.PlayerTakaDamage(1);
                player = collision.transform;
                collision.transform.position = transform.position;
                collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                skin.GetComponent<Animator>().Play("Stuck");
                GetComponent<BoxCollider2D>().enabled = false;
                collision.GetComponent<PlayerController>().enabled = false;
                Invoke("ReleasePlayer", 1f);
            }
        }
        void ReleasePlayer()
        {
            player.GetComponent<PlayerController>().enabled = true;
        }


    }
}

