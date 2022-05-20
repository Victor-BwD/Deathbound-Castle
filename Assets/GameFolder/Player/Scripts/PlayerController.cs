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

    int comboNumber;
    float timeCombo;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        caractersController = GetComponent<Caracters>();
        
    }

    // Update is called once per frame
    void Update()
    {
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
            skin.GetComponent<Animator>().Play("PlayerAttack" + comboNumber, -1);
        }
        
        if(timeCombo >= 1)
        {
            comboNumber = 0;
        }

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
            skin.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
            skin.GetComponent<Animator>().SetBool("PlayerRun", true);
        }
        else
        {
            skin.GetComponent<Animator>().SetBool("PlayerRun", false);
        }
    }
}
