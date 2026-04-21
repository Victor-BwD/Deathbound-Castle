using Core.Characters;
using Core.Services;
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
        private HealthComponent playerHealth;
    
    
        void Start()
        {
            // Obter referência do Player pelo tag
            if (player == null)
            {
                var playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    player = playerObj.transform;
                }
            }

            // Obter HealthComponent do Player
            if (player != null)
            {
                playerHealth = player.GetComponent<HealthComponent>();
                if (playerHealth == null)
                {
                    Debug.LogError("BearTrap: Player não tem HealthComponent!");
                }
            }
            else
            {
                Debug.LogError("BearTrap: Player não encontrado!");
            }
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (playerHealth == null)
                {
                    Debug.LogWarning("BearTrap: playerHealth é null!");
                    return;
                }

                // Fazer dano
                if (_audioSource != null && bearTrapAudio != null)
                {
                    _audioSource.PlayOneShot(bearTrapAudio);
                }
                
                playerHealth.TakeDamage(1);
                
                // Stun o Player
                player = collision.transform;
                collision.transform.position = transform.position;
                
                var rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                }
                
                if (skin != null)
                {
                    var animator = skin.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.Play("Stuck");
                    }
                }
                
                GetComponent<BoxCollider2D>().enabled = false;
                
                var playerController = collision.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.enabled = false;
                }
                
                Invoke("ReleasePlayer", 1f);
            }
        }

        void ReleasePlayer()
        {
            if (player != null)
            {
                var playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.enabled = true;
                }
            }
        }


    }
}

