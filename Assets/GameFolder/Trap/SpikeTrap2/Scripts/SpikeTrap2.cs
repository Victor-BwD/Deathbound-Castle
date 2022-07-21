using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap2 : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private Characters characters;


    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        characters = GameObject.Find("Player").GetComponent<Characters>();

    }

    // Update is called once per frame
    void Update()
    {

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
