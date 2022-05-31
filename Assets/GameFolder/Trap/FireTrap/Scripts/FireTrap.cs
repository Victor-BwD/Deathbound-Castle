using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    private Caracters caracterScript;


    // Start is called before the first frame update
    void Start()
    {
        caracterScript = GameObject.Find("Player").GetComponent<Caracters>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            caracterScript.life--;

            if (caracterScript.life <= 0)
            {
                this.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
