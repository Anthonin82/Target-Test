using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float horizontalSpeed;
    [HideInInspector] float defaultGravityScale;

    public LayerMask wallLayer;
    public float wallDetectionHorizontalDistance = 0.1f;
    public float wallDetectionVerticalDistance = 1f;

    public bool onLeftWall;
    public bool onRightWall;
    public bool onWall;

    public bool onGround;   

    public bool isJumping = false;
    bool pressingJump;
    public bool reachedJumpApexThisFrame = false;    
    public float jumpForce = 200f;

    public Transform leftPlayerSide;
    public Transform rightPlayerSide;
    public Transform bottomPlayerSide;

    bool pressingLeft;
    bool pressingRight;
    bool pressingUp;
    bool pressingDown;

    
    
    public bool pressingDash;
    public float DashSpeed;
    public float DashDistance;        
    public bool DashAvailable = false;
    public bool isDashing = false;
    public float CoefSpeedDashDown;

    private void Awake()
    {
        defaultGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        pressingLeft = Input.GetKey(KeyCode.A);
        pressingRight = Input.GetKey(KeyCode.D);
        pressingUp = Input.GetKey(KeyCode.W);
        pressingDown = Input.GetKey(KeyCode.S);

        pressingJump = Input.GetKey(KeyCode.P);
        pressingDash = Input.GetKey(KeyCode.Space);
        

        if (Input.GetKeyUp(KeyCode.P) && isJumping)
        {            
            reachedJumpApexThisFrame = true;
        }
    }


    private void FixedUpdate()
    {      

        onLeftWall = Physics2D.BoxCast(leftPlayerSide.position, new Vector2(wallDetectionHorizontalDistance, wallDetectionVerticalDistance), 0, Vector2.left, 0, wallLayer);
        onRightWall = Physics2D.BoxCast(rightPlayerSide.position, new Vector2(wallDetectionHorizontalDistance, wallDetectionVerticalDistance), 0, Vector2.right, 0, wallLayer);
        onWall = onLeftWall || onRightWall;

        if (!onGround && Physics2D.Raycast(bottomPlayerSide.position, Vector2.down, 0.01f, wallLayer))
        {
            LandingOnGround();
        }

        onGround = Physics2D.Raycast(bottomPlayerSide.position, Vector2.down, 0.01f, wallLayer);
        if (onGround)
        { 
            DashAvailable = true;
        }

        if (pressingLeft && !pressingRight && !onLeftWall && !isDashing)
        {
            rb.velocity = new Vector2(-horizontalSpeed, rb.velocity.y);
        }
        else if (pressingRight && !pressingLeft && !onRightWall && !isDashing)
        {
            rb.velocity = new Vector2(horizontalSpeed, rb.velocity.y);
        }
        else if (!pressingLeft && !pressingRight && !isDashing)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (pressingJump && onGround && !isJumping)// could be onGround and jumping, since the onGround check could be a bit too wide
        {
            isJumping = true;
            rb.AddForce(Vector2.up * jumpForce);
            rb.gravityScale = 0.2f * defaultGravityScale;

        }

        if (reachedJumpApexThisFrame)
        {
            isJumping = false;
            reachedJumpApexThisFrame = false;
            rb.gravityScale = 1 * defaultGravityScale;
        }
        if ( pressingDown && !isJumping && !onGround)
        {
            rb.gravityScale = 2 * defaultGravityScale;
        }

        if (pressingDash && !onWall && DashAvailable && !isDashing)
        {
            isDashing = true;
            rb.gravityScale = 0;
            Vector2 DashAngle = Vector2.zero;
            if (pressingRight && !pressingLeft)
            {
                DashAngle.x = 1;
            }
            else if (pressingLeft && !pressingRight)
            {
                DashAngle.x = -1;
            }
            if (pressingUp && !pressingDown)
            {
                DashAngle.y = 1;
            }
            else if (pressingDown && !pressingUp)
            {
                DashAngle.y = -1;
            }
            if (DashAngle.y == -1 && DashAngle.x != 0 && !onGround ) 
            {
                DashAngle = DashAngle * CoefSpeedDashDown;
            }
            rb.velocity = new Vector2(DashAngle.x * DashSpeed, DashAngle.y*DashSpeed);
            DashAvailable = false;
            StartCoroutine("DashTimer");      
                       
        }        


    }
    public void LandingOnGround()
    {
        rb.gravityScale = defaultGravityScale;
    }
    public IEnumerator DashTimer()
    {
        
        yield return new WaitForSeconds(DashDistance/DashSpeed);          
        rb.velocity =  new Vector2 (0, rb.velocity.y);        
        isDashing = false;
        rb.gravityScale = defaultGravityScale; 


    }




}
