using System;
using GameFolder.Scripts;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Characters))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform floorCollider;
        [SerializeField] private Transform skin;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private int jumpPower = 13;
        [SerializeField] private int dashPower = 30;
        [SerializeField] private float speedXMultiply = 7f;
        [SerializeField] private float gravityScale = 2.2f;
        [SerializeField] private float maxFallSpeed = 14f;
        [SerializeField] private float fallMultiplier = 2.2f;
        [SerializeField] private float lowJumpMultiplier = 2.8f;

        public LayerMask floorLayer;

        private Rigidbody2D rb;
        private Collider2D bodyCollider;
        private Animator receiveSkinAnimator;
        private Characters charactersController;
        private float dashCooldown;
        private float dashCooldownMax = 2f;
        private PlayerController playerController;
        
        private float horizontalInput;
        private bool jumpInputBuffer;
        private bool jumpHeld;
        private bool dashInputBuffer;
        private bool isGrounded;
        private int dashDirection = 1;

        private bool isDashing;
        private float dashTimeLeft;
        private float lastGroundCheckTime;
        private bool previousGroundedState;

        [SerializeField] private float coyoteTime = 0.1f;
        [SerializeField] private float groundProbeHeight = 0.08f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            bodyCollider = GetComponent<Collider2D>();
            charactersController = GetComponent<Characters>();
            receiveSkinAnimator = skin.GetComponent<Animator>();
            playerController = GetComponent<PlayerController>();    

            if (rb == null || bodyCollider == null || charactersController == null || receiveSkinAnimator == null)
            {
                Debug.LogError("PlayerMovement: Missing required components!");
                enabled = false;
                return;
            }

            rb.gravityScale = gravityScale;
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
            ApplyBetterGravity();
            ApplyFallClamp();
        }

        private void CacheInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            jumpInputBuffer = Input.GetButtonDown("Jump");
            jumpHeld = Input.GetButton("Jump");
            dashInputBuffer = Input.GetButtonDown("Fire2");
        }

        private void UpdateGroundCheck()
        {
            Bounds bounds = bodyCollider.bounds;
            Vector2 probeCenter = new Vector2(bounds.center.x, bounds.min.y - (groundProbeHeight * 0.5f));
            Vector2 probeSize = new Vector2(Mathf.Max(0.05f, bounds.size.x * 0.8f), groundProbeHeight);

            bool groundedByFeetProbe = Physics2D.OverlapBox(probeCenter, probeSize, 0f, floorLayer);

            // Only accept grounded while falling/settled to avoid false states around head hits.
            isGrounded = groundedByFeetProbe && rb.velocity.y <= 0.05f;

            if (isGrounded)
            {
                lastGroundCheckTime = Time.time;
            }

            if (isGrounded != previousGroundedState)
            {
                previousGroundedState = isGrounded;
            }
        }

        private void ProcessJumpInput()
        {
            if (!jumpInputBuffer) return;
            
            if (isGrounded || (Time.time - lastGroundCheckTime < coyoteTime))
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

        private void ApplyFallClamp()
        {
            if (isGrounded)
            {
                return;
            }

            float clampedY = Mathf.Max(rb.velocity.y, -Mathf.Abs(maxFallSpeed));
            if (!Mathf.Approximately(clampedY, rb.velocity.y))
            {
                rb.velocity = new Vector2(rb.velocity.x, clampedY);
            }
        }

        private void ApplyBetterGravity()
        {
            if (isGrounded)
            {
                return;
            }

            float extraGravityMultiplier = 0f;

            if (rb.velocity.y < 0f)
            {
                extraGravityMultiplier = Mathf.Max(0f, fallMultiplier - 1f);
            }
            else if (rb.velocity.y > 0f && !jumpHeld)
            {
                extraGravityMultiplier = Mathf.Max(0f, lowJumpMultiplier - 1f);
            }

            if (extraGravityMultiplier <= 0f)
            {
                return;
            }

            float gravityDeltaY = Physics2D.gravity.y * extraGravityMultiplier * Time.fixedDeltaTime;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + gravityDeltaY);
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