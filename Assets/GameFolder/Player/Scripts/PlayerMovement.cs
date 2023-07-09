using System;
using GameFolder.Scripts;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Characters))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Vector2 speed;
        [SerializeField] private Transform floorCollider;
        [SerializeField] private Transform skin;

        public LayerMask floorLayer;

        private Rigidbody2D rb;
        private Animator receiveSkinAnimator; // Variable to receive animator from the skin game object
        private Characters charactersController;
        private int dashPower = 800;
        private float dashTime;
        private int jumpPower = 1100;
        private PlayerController playerController;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            charactersController = GetComponent<Characters>();
            receiveSkinAnimator = skin.GetComponent<Animator>();
            playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            dashTime += Time.deltaTime;
            CheckDash();
            JumpCheck();
        }

        private void FixedUpdate()
        {
            speed = new Vector2(Input.GetAxisRaw("Horizontal") * 7f, rb.velocity.y); // Global var to receive speed in X with input 

            // If to check if player has used dash or not
            if (dashTime > 0.4)
            {
                rb.velocity = speed; // rb velocity receive speed
            }
            
            WalkAnimation(); 
        }

        private void WalkAnimation()
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                skin.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1); // Flip
                receiveSkinAnimator.SetBool("PlayerRun", true); // True in animation run
            }
            else
            {
                receiveSkinAnimator.SetBool("PlayerRun", false); // False in animation run
            }
        }

        private void JumpCheck()
        {
            bool canJump = Physics2D.OverlapCircle(floorCollider.position, 0.1f, floorLayer);
            if (canJump && Input.GetButtonDown("Jump"))
            {
                receiveSkinAnimator.Play("PlayerJump", -1); // -1 search for all layers
                rb.velocity = Vector2.zero;

                rb.AddForce(new Vector2(0, jumpPower));
            }
        }

        private void CheckDash()
        {
            if (Input.GetButtonDown("Fire2") && dashTime >= 2)
            {
                dashTime = 0;
                receiveSkinAnimator.Play("PlayerDash", -1); // -1 search for all layers
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(skin.localScale.x * dashPower, 0));
                SoundManager.Instance.Play(playerController.AudioPlayer.dashSound);
            }
        }
    }
}