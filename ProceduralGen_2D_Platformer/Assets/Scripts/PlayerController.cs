using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;

    [SerializeField] private LayerMask platformLayerMask;

    private bool isGrounded;
    private bool pressingSpace;
    private bool isRunning;


    private float movementDirection;
    public float runSpeed;
    public float walkSpeed;
    public float accelPower;


    private float jumpCoyoteTimeCounter;
    private float jumpBufferCounter;

    public float jumpCoyoteTime;
    public float jumpBuffer;
    public float jumpForce;


    public float acceleration;
    public float fallForce;
    public float frictionAmount;

    private Vector3 colliderSize;
    private Vector3 colliderCenter;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        colliderSize = GetComponent<Collider2D>().bounds.size;
        colliderCenter = GetComponent<Collider2D>().bounds.center;


    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInputs();
    }

    private void FixedUpdate()
    {
        ApplyMovementInputs();
    }








    public void ApplyMovementInputs()
    {
        Move();
        Jump();
    }



    public void Move()
    {
        // set the maximum speed based on if the player is running or walking
        float maxSpeed;
        if (isRunning)
        {
            maxSpeed = runSpeed;

        }
        else
        {
            maxSpeed = walkSpeed;
        }

        float targetSpeed = maxSpeed * movementDirection; // get the target speed with direction
        float speedDif = targetSpeed - playerRigidbody.velocity.x;

        // as the players velocity gets closer to the target speed(or speedDif -> 0), the movement speed gets lessened exponentially.
        // acceleration value hastens the time in which movement speed approaches the target speed linearly
        // accelPower value hastens exponentially


        float movementSpeed = Mathf.Pow(Mathf.Abs(speedDif) * acceleration, accelPower) * Mathf.Sign(speedDif);

        // apply calculated force to the player
        playerRigidbody.AddForce(
                new Vector2((movementSpeed * Time.deltaTime), 0));

        ApplyFriction();


    }

    private void ApplyFriction()
    {
        //Check if grounded and not using lef/right inputs
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

        /*playerRigidbody.AddForce(
                new Vector2(0, (4 * jumpForce * Time.deltaTime)));*/
        // playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
        playerRigidbody.velocity += (jumpForce * Time.deltaTime) * Vector2.up;

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

    private void ApplyIncreasedFallSpeed()
    {
        if (!isGrounded)
        {
            playerRigidbody.velocity += (fallForce * Time.deltaTime) * Vector2.down;
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
        ApplyIncreasedFallSpeed();




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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        // - or + Value of the horizontal axis
        movementDirection = Input.GetAxisRaw("Horizontal");

    }

    public bool IsGrounded()
    {
        // Create a ray box near bottom of player, return true if the ray is colliding with "Ground" LayerMask
        float extraHeightTest = 0.1f;
        Vector3 colliderSize = GetComponent<Collider2D>().bounds.size;
        Vector3 colliderCenter = GetComponent<Collider2D>().bounds.center;

        // Make ground collider only take up half of the player
        colliderCenter.y = colliderCenter.y - (colliderSize.y / 4f);
        colliderSize.y = colliderSize.y / 2f;


        RaycastHit2D rayHit = Physics2D.BoxCast(colliderCenter, colliderSize, 0f, Vector2.down, extraHeightTest, platformLayerMask);
        //Debug.Log(rayHit.collider);
        return rayHit.collider != null;


    }
}
