using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Traps
{
    public class FireTrap : MonoBehaviour
    {
        private Characters characters;
        private PlayerHealth playerHealth;


        // Start is called before the first frame update
        void Start()
        {
            characters = GameObject.Find("Player").GetComponent<Characters>();
            playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
       
        }
    

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerHealth.PlayerTakaDamage(1);

                if (characters.life <= 0)
                {
                    this.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }
}

