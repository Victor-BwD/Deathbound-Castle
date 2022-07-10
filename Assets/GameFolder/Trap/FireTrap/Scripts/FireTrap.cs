using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    private Characters characterScript;


    // Start is called before the first frame update
    void Start()
    {
        characterScript = GameObject.Find("Player").GetComponent<Characters>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            characterScript.life--;

            if (characterScript.life <= 0)
            {
                this.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
