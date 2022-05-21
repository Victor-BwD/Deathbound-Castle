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
    Caracters caractersController;
    Animator receiveSkinAnimator; // Variable to receive animator from the skin game object

    int dashPower = 150;
    int jumpPower = 150;
    public int comboNumber;
    float timeCombo;
    float dashTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        caractersController = GetComponent<Caracters>();
        receiveSkinAnimator = skin.GetComponent<Animator>(); // Get animator from the skin
    }

    // Update is called once per frame
    void Update()
    {
        dashTime += Time.deltaTime;
        if (Input.GetButtonDown("Fire2") && dashTime > 2) 
        {
            dashTime = 0;
            receiveSkinAnimator.Play("PlayerDash", -1); // -1 search for all layers
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(skin.localScale.x * dashPower, 0));
        }

        if(caractersController.life <= 0)
        {
            this.enabled = false;
        }

        timeCombo += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && timeCombo > 0.5f)
        {
            comboNumber++;
            if(comboNumber > 2)
            {
                comboNumber = 1;
            }
            timeCombo = 0;
            receiveSkinAnimator.Play("PlayerAttack" + comboNumber, -1);
        }
        
        if(timeCombo >= 1)
        {
            comboNumber = 0;
        }

        bool canJump = Physics2D.OverlapCircle(floorCollider.position, 0.1f, floorLayer);
        if (canJump && Input.GetButtonDown("Jump"))
        {
            receiveSkinAnimator.Play("PlayerJump", -1); // -1 search for all layers
            rb.velocity = Vector2.zero;
            
            rb.AddForce(new Vector2(0, jumpPower));
        }



    }

    private void FixedUpdate()
    {
        speed = new Vector2(Input.GetAxisRaw("Horizontal"), rb.velocity.y); // Gloval var to receive speed in X with input 

        // If to check if player has used dash or not
        if(dashTime > 0.4)
        {
            rb.velocity = speed; // rb velocity receive speed
        }
        

        // If horizontal axis are =/= 0
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            skin.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1); // Flip
            skin.GetComponent<Animator>().SetBool("PlayerRun", true); // True in animation run
        }
        else
        {
            skin.GetComponent<Animator>().SetBool("PlayerRun", false); // False in animation run
        }
    }
}
