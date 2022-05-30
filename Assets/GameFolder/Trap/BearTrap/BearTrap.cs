using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BearTrap : MonoBehaviour
{
    Transform player;
    [SerializeField] Transform skin;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
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
