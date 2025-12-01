using System;
using GameFolder.Scripts;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Characters))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform floorCollider;
        [SerializeField] private Transform skin;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private int jumpPower = 25;
        [SerializeField] private int dashPower = 100;
        [SerializeField] private float speedXMultiply = 7f;

        public LayerMask floorLayer;

        private Rigidbody2D rb;
        private Animator receiveSkinAnimator;
        private Characters charactersController;
        private float dashCooldown;
        private float dashCooldownMax = 2f;
        private PlayerController playerController;
        
        private float horizontalInput;
        private bool jumpInputBuffer;
        private bool dashInputBuffer;
        private bool isGrounded;
        private float groundCheckRadius = 0.1f;
        private int dashDirection = 1;

        private bool isDashing;
        private float dashTimeLeft;
        private float lastGroundCheckTime;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            charactersController = GetComponent<Characters>();
            receiveSkinAnimator = skin.GetComponent<Animator>();
            playerController = GetComponent<PlayerController>();    

            if (rb == null || charactersController == null || receiveSkinAnimator == null)
            {
                Debug.LogError("PlayerMovement: Missing required components!");
                enabled = false;
            }
        }

        private void Update()
        {
            CacheInput();
            UpdateGroundCheck();
            ProcessJumpInput();
            ProcessDashInput();
            UpdateAnimations();
            UpdateDashCooldown();

            if (horizontalInput != 0)
            {
                dashDirection = (int)horizontalInput;
            }
        }

        private void FixedUpdate()
        {
            if (isDashing)
            {
                dashTimeLeft -= Time.fixedDeltaTime;
                if (dashTimeLeft <= 0)
                {
                    isDashing = false;
                    rb.velocity = Vector2.zero;
                }

                return;
            }

            ApplyMovement();
        }

        private void CacheInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            jumpInputBuffer = Input.GetButtonDown("Jump");
            dashInputBuffer = Input.GetButtonDown("Fire2");
        }

        private void UpdateGroundCheck()
        {
            isGrounded = Physics2D.OverlapCircle(floorCollider.position, groundCheckRadius, floorLayer);
            if (isGrounded)
            {
                lastGroundCheckTime = Time.time;
            }
        }

        private void ProcessJumpInput()
        {
            if (!jumpInputBuffer) return;
            
            if (isGrounded || (Time.time - lastGroundCheckTime < 0.1f))
            {
                Jump();
            }
        }

        private void ProcessDashInput()
        {
            if (!dashInputBuffer || dashCooldown > 0) return;

            Dash();
        }

        private void Jump()
        {
            receiveSkinAnimator.Play("PlayerJump", -1);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            lastGroundCheckTime = -1f;
        }

        private void Dash()
        {
            isDashing = true;
            dashTimeLeft = dashDuration;
            dashCooldown = dashCooldownMax;
            receiveSkinAnimator.Play("PlayerDash", -1);
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(dashDirection * dashPower, 0), ForceMode2D.Impulse);
            
            if (SoundManager.Instance != null && playerController != null && playerController.AudioPlayer != null)
            {
                SoundManager.Instance.Play(playerController.AudioPlayer.dashSound);
            }
        }

        private void ApplyMovement()
        {
            float targetVelocityX = horizontalInput * speedXMultiply;
            rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);
        }

        private void UpdateAnimations()
        {
            bool isMoving = horizontalInput != 0;

            if (isMoving)
            {
                skin.localScale = new Vector3(horizontalInput, 1, 1);
                receiveSkinAnimator.SetBool("PlayerRun", true);
            }
            else
            {
                receiveSkinAnimator.SetBool("PlayerRun", false);
            }
        }

        private void UpdateDashCooldown()
        {
            if (dashCooldown > 0)
            {
                dashCooldown -= Time.deltaTime;
            }
        }

        public void IncreaseSpeed()
        {
            speedXMultiply += 3f;
            Debug.Log("Velocidade aumentada: " + speedXMultiply);
        }
    }
}