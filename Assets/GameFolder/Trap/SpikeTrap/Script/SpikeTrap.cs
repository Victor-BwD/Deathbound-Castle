using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private Caracters caracterScript;
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        caracterScript = GameObject.Find("Player").GetComponent<Caracters>();
        rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, 150));
            caracterScript.life--;

            if (caracterScript.life <= 0)
            {
                this.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
