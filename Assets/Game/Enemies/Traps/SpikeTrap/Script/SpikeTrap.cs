using System.Collections;
using System.Collections.Generic;
using Core.Characters;
using UnityEngine;

namespace Traps
{
    public class SpikeTrap : MonoBehaviour
    {
        private HealthComponent playerHealth;
        private Rigidbody2D playerRb;
    
        // Start is called before the first frame update
        void Start()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerHealth = playerObj.GetComponent<HealthComponent>();
                playerRb = playerObj.GetComponent<Rigidbody2D>();
                
                if (playerHealth == null)
                {
                    Debug.LogError("SpikeTrap: Player não tem HealthComponent!");
                }
                if (playerRb == null)
                {
                    Debug.LogError("SpikeTrap: Player não tem Rigidbody2D!");
                }
            }
            else
            {
                Debug.LogError("SpikeTrap: Player não encontrado!");
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (playerRb != null)
                {
                    playerRb.linearVelocity = Vector2.zero;
                    playerRb.AddForce(new Vector2(0, 150));
                }

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }

                this.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}

