using System.Collections;
using System.Collections.Generic;
using Core.Characters;
using Player;
using UnityEngine;

namespace Traps
{
    public class FireTrap : MonoBehaviour
    {
        private HealthComponent playerHealth;


        // Start is called before the first frame update
        void Start()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerHealth = playerObj.GetComponent<HealthComponent>();
                if (playerHealth == null)
                {
                    Debug.LogError("FireTrap: Player não tem HealthComponent!");
                }
            }
            else
            {
                Debug.LogError("FireTrap: Player não encontrado!");
            }
        }
    

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }
            }
        }
    }
}

