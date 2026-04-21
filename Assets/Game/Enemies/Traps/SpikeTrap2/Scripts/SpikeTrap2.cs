using Core.Characters;
using Player;
using UnityEngine;

namespace Traps
{
    public class SpikeTrap2 : MonoBehaviour
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
                    Debug.LogError("SpikeTrap2: Player não tem HealthComponent!");
                }
            }
            else
            {
                Debug.LogError("SpikeTrap2: Player não encontrado!");
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
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }

                this.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}

