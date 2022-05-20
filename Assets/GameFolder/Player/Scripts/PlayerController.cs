using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    Vector2 speed;
    
    public Transform floorCollider;
    public LayerMask floorLayer;

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
            rb.velocity = Vector2.zero;
            
            rb.AddForce(new Vector2(0, 100));
        }

    }

    private void FixedUpdate()
    {
        speed = new Vector2(Input.GetAxisRaw("Horizontal"), rb.velocity.y);

        rb.velocity = speed;

        
    }
}
