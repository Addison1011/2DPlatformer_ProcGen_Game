using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;

    [SerializeField] private LayerMask platformLayerMask;

    private bool isGrounded {get; set;}
    private bool pressingSpace {get; set;}
    private float movementDirection { get; set; }

    public float jumpForce;
    public float fallForce;
    public float maxFallSpeed;
    public float frictionAmount;


    public float movementSpeed;
    public float accelPower;
    public float acceleration;


    private float jumpCoyoteTimeCounter;
    private float jumpBufferCounter;

    public float jumpCoyoteTime;
    public float jumpBuffer;
    


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInputs();
    }

    private void FixedUpdate()
    {
        ApplyMovementInputs();
        ClampFallSpeed();
    }


    public void ApplyMovementInputs()
    {
        Move();
        Jump();
    }



    public void Move()
    {
        float targetSpeed = movementSpeed * movementDirection; // get the target speed with direction
        float speedDif = targetSpeed - playerRigidbody.velocity.x;

        // as the players velocity gets closer to the target speed(or speedDif -> 0), the movement speed gets lessened exponentially.
        // acceleration value hastens the time in which movement speed approaches the target speed linearly
        // accelPower value hastens exponentially

        float calculatedForce = Mathf.Pow(Mathf.Abs(speedDif) * acceleration, accelPower) * Mathf.Sign(speedDif);

        // apply calculated force to the player
        playerRigidbody.AddForce(
                new Vector2((calculatedForce * Time.deltaTime), 0));

        ApplyFriction();


    }

    private void ApplyFriction()
    {
        //Check if grounded and not using left/right inputs
        if (movementDirection == 0 && IsGrounded())
        {
            // use friction ammount or velocity depending on which one is bigger, also creates a slow stop
            float negativeForceAmount = Mathf.Min(Mathf.Abs(playerRigidbody.velocity.x), Mathf.Abs(frictionAmount));

            //apply the correct direction
            negativeForceAmount *= Mathf.Sign(playerRigidbody.velocity.x);

            // applys force aginst movement direction
            playerRigidbody.AddForce(Vector2.right * -negativeForceAmount, ForceMode2D.Impulse);
        }
    }


    private void ApplyJumpVelocity()
    {
        // zero out vertical velocity before adding jump velocity
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
        playerRigidbody.AddForce(new Vector2(0, jumpForce));

        //playerRigidbody.velocity += jumpForce * Vector2.up;

    }

    private void ClampFallSpeed()
    {
        if (playerRigidbody.velocity.y <= -maxFallSpeed)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, -maxFallSpeed);
        }
    }


    private void ApplyJumpCoyoteTime()
    {

        if (IsGrounded())
        {
            jumpCoyoteTimeCounter = jumpCoyoteTime;
        }
        else
        {
            jumpCoyoteTimeCounter -= Time.deltaTime;
        }

    }

    private void ApplyFallSpeed()
    {
        if (!isGrounded)
        {
            // If player is falling and the downward vertical velocity is greater than maxFallSpeed then clamp the fall speed
            // Otherwise apply increased fall speed
            if (playerRigidbody.velocity.y <= -maxFallSpeed)
            {
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, -maxFallSpeed);
            }
            else
            {
                playerRigidbody.velocity += (fallForce * Time.deltaTime) * Vector2.down;
            }
           
        }
    }

    private void ApplyJumpBuffer()
    {
        if (pressingSpace)
        {
            jumpBufferCounter = jumpBuffer;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    public void Jump()
    {



        ApplyJumpCoyoteTime();
        ApplyJumpBuffer();
        ApplyFallSpeed();




        if (jumpBufferCounter >= 0 && jumpCoyoteTimeCounter >= 0)
        {
            ApplyJumpVelocity();


            jumpBufferCounter = 0;
            jumpCoyoteTimeCounter = 0;

        }

    }

    public void GetMovementInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressingSpace = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {

            pressingSpace = false;
        }

        // - or + Value of the horizontal axis
        movementDirection = Input.GetAxisRaw("Horizontal");

    }

    public bool IsGrounded()
    {
        
        float extraHeightTest = 0.1f;
        Vector3 colliderSize = playerCollider.bounds.size;
        Vector3 colliderCenter = playerCollider.bounds.center;

        // Make ray box only take up the bottom half of the player
        colliderCenter.y -= (colliderSize.y / 4f);
        colliderSize.y /= 2f;

        // make ray box width a little less than player width so the player cant jump when hugging a wall
        colliderSize.x = colliderSize.x - 0.2f;

        // Create a ray box, return true if the ray is colliding with "Ground" LayerMask
        RaycastHit2D rayHit = Physics2D.BoxCast(colliderCenter, colliderSize, 0f, Vector2.down, extraHeightTest, platformLayerMask);

        return rayHit.collider != null;


    }
}
