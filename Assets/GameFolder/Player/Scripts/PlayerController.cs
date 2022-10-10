using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour {
    [SerializeField] private Vector2 speed;
    [SerializeField] private Transform floorCollider;
    [SerializeField] private Transform skin;

    public LayerMask floorLayer;
    public int comboNumber;

    private Rigidbody2D rb;
    private Characters charactersController;
    private Animator receiveSkinAnimator; // Variable to receive animator from the skin game object
    private int dashPower = 800;
    private int jumpPower = 1100;
    private float timeCombo;
    private float dashTime;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        charactersController = GetComponent<Characters>();
        receiveSkinAnimator = skin.GetComponent<Animator>();
    }

    void Update() {
        dashTime += Time.deltaTime;
        CheckDash();

        DisableControls();

        timeCombo += Time.deltaTime;
        ComboCheck();

        JumpCheck();
    }

    private void DisableControls()
    {
        if (charactersController.life <= 0) {
            rb.simulated = false;
            this.enabled = false;
        }
    }

    private void JumpCheck()
    {
        bool canJump = Physics2D.OverlapCircle(floorCollider.position, 0.1f, floorLayer);
        if (canJump && Input.GetButtonDown("Jump")) {
            receiveSkinAnimator.Play("PlayerJump", -1); // -1 search for all layers
            rb.velocity = Vector2.zero;

            rb.AddForce(new Vector2(0, jumpPower));
        }
    }

    private void ComboCheck()
    {
        if (Input.GetButtonDown("Fire1") && timeCombo > 0.5f) {
            comboNumber++;
            if (comboNumber > 2) {
                comboNumber = 1;
            }

            timeCombo = 0;
            receiveSkinAnimator.Play("PlayerAttack" + comboNumber, -1);
        }

        if (timeCombo >= 1) {
            comboNumber = 0;
        }
    }

    private void CheckDash()
    {
        if (Input.GetButtonDown("Fire2") && dashTime > 2)
        {
            dashTime = 0;
            receiveSkinAnimator.Play("PlayerDash", -1); // -1 search for all layers
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(skin.localScale.x * dashPower, 0));
        }
    }

    private void FixedUpdate()
    {
        speed = new Vector2(Input.GetAxisRaw("Horizontal") * 7f,
            rb.velocity.y); // Gloval var to receive speed in X with input 

        // If to check if player has used dash or not
        if (dashTime > 0.4) {
            rb.velocity = speed; // rb velocity receive speed
        }

        WalkAnimation();
    }

    private void WalkAnimation()
    {
        if (Input.GetAxisRaw("Horizontal") != 0) {
            skin.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1); // Flip
            receiveSkinAnimator.SetBool("PlayerRun", true); // True in animation run
        }
        else {
            receiveSkinAnimator.SetBool("PlayerRun", false); // False in animation run
        }
    }
}
}
