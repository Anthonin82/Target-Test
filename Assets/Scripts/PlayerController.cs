using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer PlayerSR;
    public Rigidbody2D rb;
    public float horizontalSpeed;
    [HideInInspector] float defaultGravityScale;
    public float TimerGravityReset;
    public float JumpingGravityCoefficient;

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
    public float WallJumpHorizontalForce = 200f;
    public float TimerJumpApex;
    public bool Gravityneeded;
    public bool DoubleJumpAvailable;

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

    public bool RecentlyOnleftWall;
    public bool RecentlyOnRightWall;
    public bool InterdictionLeft;
    public bool InterdictionRight;
    public float TimerInterdiction;
    public float WalljumpVerticalForceModifier;

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

        
        

        if (!pressingDash) 
        {
            pressingDash = Input.GetKeyDown(KeyCode.Space);
        }
        if (!pressingJump)
        {
            pressingJump = Input.GetKeyDown(KeyCode.P);
        }
        
        if (Input.GetKeyUp(KeyCode.P) && isJumping)
        {            
            reachedJumpApexThisFrame = true;
        }

    }


    private void FixedUpdate()
    {
        

        if (onLeftWall && !Physics2D.BoxCast(leftPlayerSide.position, new Vector2(wallDetectionHorizontalDistance, wallDetectionVerticalDistance), 0, Vector2.left, 0, wallLayer) )//on rentre la dedans quand on quitte le mur cette frame-ci
        {
            
            StartCoroutine(ResetOnWall("left"));
        }
        if (onRightWall && !Physics2D.BoxCast(rightPlayerSide.position, new Vector2(wallDetectionHorizontalDistance, wallDetectionVerticalDistance), 0, Vector2.right, 0, wallLayer ))
        {
            StartCoroutine(ResetOnWall("right"));
        }

        onLeftWall = Physics2D.BoxCast(leftPlayerSide.position, new Vector2(wallDetectionHorizontalDistance, wallDetectionVerticalDistance), 0, Vector2.left, 0, wallLayer);
        onRightWall = Physics2D.BoxCast(rightPlayerSide.position, new Vector2(wallDetectionHorizontalDistance, wallDetectionVerticalDistance), 0, Vector2.right, 0, wallLayer);
        onWall = onLeftWall || onRightWall;

        if (onLeftWall)
        {
            RecentlyOnleftWall = true;
        }

        if (onRightWall)
        {
            RecentlyOnRightWall = true;
        }


        if (!onGround && Physics2D.Raycast(bottomPlayerSide.position, Vector2.down, 0.01f, wallLayer))
        {
            LandingOnGround();
        }

        onGround = Physics2D.Raycast(bottomPlayerSide.position, Vector2.down, 0.01f, wallLayer);
        
        if (pressingLeft && !pressingRight && !onLeftWall && !isDashing && !InterdictionLeft)
        {
            rb.velocity = new Vector2(-horizontalSpeed, rb.velocity.y);
        }
        else if (pressingRight && !pressingLeft && !onRightWall && !isDashing && !InterdictionRight)
        {
            rb.velocity = new Vector2(horizontalSpeed, rb.velocity.y);
        }
        else if (!pressingLeft && !pressingRight && !isDashing)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (pressingJump  && !isJumping )// could be onGround and jumping, since the onGround check could be a bit too wide
        {
            if (onGround)
            {
                StartCoroutine("JumpTimer");
                isJumping = true;
                rb.AddForce(Vector2.up * jumpForce);
                rb.gravityScale = JumpingGravityCoefficient * defaultGravityScale;
            }         
            if (!onGround)
            {
                 
                if (RecentlyOnleftWall && pressingRight)
                {
                    InterdictionLeft = true;
                    StartCoroutine("TimerInterdictionLeft");
                    rb.gravityScale = JumpingGravityCoefficient * defaultGravityScale;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(WallJumpHorizontalForce, jumpForce * WalljumpVerticalForceModifier));
                    isJumping = false;
                    GravityReset();
                    StartCoroutine("ColorChangeWallJump");

                }
                else if (RecentlyOnRightWall && pressingLeft)
                {
                    InterdictionRight = true;
                    StartCoroutine("TimerInterdictionRight");
                    rb.gravityScale = JumpingGravityCoefficient * defaultGravityScale;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(-WallJumpHorizontalForce, jumpForce * WalljumpVerticalForceModifier));
                    isJumping = false;
                    GravityReset();
                    StartCoroutine("ColorChangeWallJump");
                }
                else if (DoubleJumpAvailable && !onWall && !isDashing)
                {
                    StopCoroutine(JumpTimer());
                    StartCoroutine("JumpTimer");
                    isJumping = true;
                    rb.gravityScale = JumpingGravityCoefficient * defaultGravityScale;
                    if (rb.velocity.y < 0)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                    }
                    rb.AddForce(Vector2.up * jumpForce);
                    DoubleJumpAvailable = false;
                }
            }
          
        }

        if (reachedJumpApexThisFrame)
        {
            isJumping = false;
            reachedJumpApexThisFrame = false;
            rb.gravityScale = defaultGravityScale;
        }
        if ( pressingDown && !isJumping && !onGround)
        {
            rb.gravityScale = JumpingGravityCoefficient * defaultGravityScale;
        }

        if (pressingDash && !onWall && DashAvailable && !isDashing)
        {
            OnDash();           
        }
        if (pressingDash)
        {
            pressingDash = Input.GetKeyDown(KeyCode.Space);
        }
        if (pressingJump)
        {
            pressingJump = Input.GetKeyDown(KeyCode.P);
        }
    }
    public void LandingOnGround()
    {
        rb.gravityScale = defaultGravityScale;
        DashAvailable = true;
        DoubleJumpAvailable = true;
    }
    public IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(TimerJumpApex);
        Gravityneeded = true; 

    }
    public IEnumerator ResetOnWall(string ValueParam)
    {
        yield return new WaitForSeconds (Time.fixedDeltaTime * 3);
        if ( ValueParam == "left")
        {
            RecentlyOnleftWall = false;
        }
        else if (ValueParam == "right") 
        { 
            RecentlyOnRightWall = false;
        }
        else
        {
            Debug.LogError("Error");
        }
         

    }
    public IEnumerator DashTimer()
    {
        
        yield return new WaitForSeconds(DashDistance/DashSpeed);          
        rb.velocity =  new Vector2 (0, rb.velocity.y); 
        isDashing = false;
        rb.gravityScale = defaultGravityScale; 


    }
    public IEnumerator ColorChangeWallJump()
    {
        PlayerSR.color = Color.black;
        yield return new WaitForSeconds(0.5f);
        PlayerSR.color = Color.white;
    }
    public IEnumerator ColorChangeDash() 
    { 
        PlayerSR.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        PlayerSR.color = Color.white;
    }
    public IEnumerator TimerInterdictionLeft()
    {
        yield return new WaitForSeconds(TimerInterdiction);
        InterdictionLeft = false;
    }
    public IEnumerator TimerInterdictionRight()
    {
        yield return new WaitForSeconds(TimerInterdiction);
        InterdictionRight = false;
    }
    public void GravityReset()
    {        
        if (isJumping && rb.velocity.y <= 0)
        { 
            rb.gravityScale= JumpingGravityCoefficient * defaultGravityScale;            
        }
        /*else if ( !isJumping && !isDashing)
        {
            rb.gravityScale = defaultGravityScale;
        }*/


        if (!isDashing && isJumping )
        {
            rb.gravityScale = JumpingGravityCoefficient * defaultGravityScale;
        }
        if (!isDashing && isJumping) 
        {
            rb.gravityScale = defaultGravityScale;
        }
        
    }
    public void OnDash()
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
        if (DashAngle.y == -1 && DashAngle.x != 0 && !onGround)
        {
            DashAngle = DashAngle * CoefSpeedDashDown;
        }
        rb.velocity = new Vector2(DashAngle.x * DashSpeed, DashAngle.y * DashSpeed);
        DashAvailable = false;
        //GravityReset();
        StartCoroutine("ColorChangeDash");
        StartCoroutine("DashTimer");

    }

}
