using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    Vector2 speed;
    
    public Transform floorCollider;
    public LayerMask floorLayer;
    public Transform skin;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        bool canJump = Physics2D.OverlapCircle(floorCollider.position, 0.1f, floorLayer);
        if (canJump && Input.GetButtonDown("Jump"))
        {
            skin.GetComponent<Animator>().Play("PlayerJump", -1); // -1 search for all layers
            rb.velocity = Vector2.zero;
            
            rb.AddForce(new Vector2(0, 150));
        }



    }

    private void FixedUpdate()
    {
        speed = new Vector2(Input.GetAxisRaw("Horizontal"), rb.velocity.y);

        rb.velocity = speed;

        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            skin.GetComponent<Animator>().SetBool("PlayerRun", true);
        }
        else
        {
            skin.GetComponent<Animator>().SetBool("PlayerRun", false);
        }
    }
}
