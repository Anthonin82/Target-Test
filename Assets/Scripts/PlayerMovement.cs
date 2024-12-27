using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //TODO :
    // Wavedash devrait cancel le dash et refresh le cd, pour qu on puisse empecher le saut pendant le dash et que ca soit pas cringe
    // Appuyer sur jump contre un mur devrait nous dégager

    public SpriteRenderer PlayerSR;
    public Rigidbody2D rb;
    public float horizontalSpeed;
    public float maxHorizontalSpeed;
    public float horizontalAccel;
    public float horizontalDecel;
    [HideInInspector] float defaultGravityScale;
    public float TimerGravityReset;
    public float JumpingGravityCoefficient;

    public Vector3 PlayerStart;
    public LayerMask wallLayer;
    public float wallDetectionHorizontalDistance = 0.1f;
    public float wallDetectionVerticalDistance = 1f;

    
    public bool onLeftWall;
    public bool onRightWall;
    public bool onWall;
    public bool onGround;   

    public bool isJumping = false;
    public bool pressingDownJump;
    public bool releasingJump;
    public bool reachedJumpApexThisFrame = false;    
    public float jumpForce = 200f;
    public float WallJumpHorizontalForce = 200f;
    public float TimerJumpApex;
    public bool DoubleJumpAvailable;

    public Transform leftPlayerSide;
    public Transform rightPlayerSide;
    public Transform bottomPlayerSide;

    public bool pressingLeft;
    public bool pressingRight;
    public bool pressingUp;
    public bool pressingDown;

    public bool pressingDownDash;        
    public float DashSpeed;
    public float DashDistance;        
    public bool DashAvailable = false;
    public bool isDashing = false;
    public float CoefSpeedDashDown;

    public bool RecentlyOnleftWall;
    public bool RecentlyOnRightWall;
    public bool forcedRightMovement;
    public bool forcedLeftMovement;
    public float TimerInterdiction;
    public float WalljumpVerticalForceModifier;

    public bool inCornerBoostState;
    public float CornerBoostDuration;    
    public float CoefCornerBoostHorizontalSpeed;

    public Animator animator;

    public static PlayerMovement instance;

    
    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("jlfl");
        }
        instance = this;
        defaultGravityScale = rb.gravityScale;
        PlayerStart = rb.position;
    }

    


    private void FixedUpdate()
    {
        animator.SetBool("isDashing", isDashing);
        animator.SetFloat("VerticalVelocity",rb.velocity.y);
        animator.SetBool("isJumping", isJumping);

        if (releasingJump && isJumping)
        {
            reachedJumpApexThisFrame = true;
        }

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
        animator.SetBool("onGround", onGround);

        float currentFrameCornerBoostCoeff;
        currentFrameCornerBoostCoeff = inCornerBoostState ? CoefCornerBoostHorizontalSpeed : 1f;

        float horizontalSpeedGoal;

        if ((pressingLeft||forcedLeftMovement) && !pressingRight && !onLeftWall && !isDashing && !forcedRightMovement)
        {
            horizontalSpeedGoal = -horizontalSpeed * currentFrameCornerBoostCoeff;
            PlayerSR.flipX = true;
        }
        else if ((pressingRight||forcedRightMovement) && !pressingLeft && !onRightWall && !isDashing && !forcedLeftMovement)
        {
            horizontalSpeedGoal = horizontalSpeed * currentFrameCornerBoostCoeff;
            PlayerSR.flipX = false;
        }
        else if (!pressingLeft && !pressingRight && !isDashing)
        {
            horizontalSpeedGoal = 0;
        }
        else
        {
            horizontalSpeedGoal = rb.velocity.x;
        }

        if(!isDashing && Mathf.Abs(rb.velocity.x) > maxHorizontalSpeed) //on cap la vitesse a la vitesse max autorisée
        {
            rb.velocity = new Vector2(maxHorizontalSpeed * Mathf.Sign(rb.velocity.x), rb.velocity.y);
        }
        
        
        if((horizontalSpeedGoal > 0 && rb.velocity.x > horizontalSpeed) || (horizontalSpeedGoal < 0 && rb.velocity.x < -horizontalSpeed))
        {
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, horizontalSpeedGoal, horizontalDecel * Time.fixedDeltaTime), rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, horizontalSpeedGoal, horizontalAccel * Time.fixedDeltaTime), rb.velocity.y);
        }
        if (pressingDownDash && DashAvailable && !isDashing)
        {
            OnDash();
        }
        if (isDashing)
        {
            if(Time.time - timeStartDash >= DashDistance / DashSpeed)
            {
                CancelDash();
            }
        }

        if (pressingDownJump  && !isJumping )// could be onGround and jumping, since the onGround check could be a bit too wide
        {



            if (onGround)
            {
                isJumping = true;
                animator.SetBool("isJumping", isJumping);
                rb.AddForce(Vector2.up * jumpForce);
                
            }
            else
            {                 
                if (RecentlyOnleftWall)
                {
                    animator.SetBool("isJumping", true);
                    forcedRightMovement = true;
                    StartCoroutine(TimerInterdictionLeft());
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(WallJumpHorizontalForce, jumpForce * WalljumpVerticalForceModifier));
                    isJumping = true;
                }
                else if (RecentlyOnRightWall)
                {
                    animator.SetBool("isJumping", true);
                    forcedLeftMovement = true;
                    StartCoroutine("TimerInterdictionRight");
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(-WallJumpHorizontalForce, jumpForce * WalljumpVerticalForceModifier));
                    isJumping = true;
                }
                else if (DoubleJumpAvailable && !onWall && !isDashing)
                {
                    
                    
                    isJumping = true;
                    animator.SetBool("isJumping", isJumping);
                    if (rb.velocity.y < 0)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                    }
                    rb.AddForce(Vector2.up * jumpForce);
                    DoubleJumpAvailable = false;

                    if((pressingRight && rb.velocity.x < 0) || (pressingLeft && rb.velocity.x > 0))
                    {
                        rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                    }

                }
            }

            if (isJumping)//dans le cas ou on est rentré dans une des boucles du dessus et qu'un jump a effectivement été perform
            {
                if (isDashing)
                {
                    CancelDash();
                }
            }

          
        }

        if (reachedJumpApexThisFrame)
        {
            isJumping = false;
            reachedJumpApexThisFrame = false;
        }

        SetGravity();

        //ces trois lignes ne doivent être vraies que pour une frame de fixed update max.
        pressingDownDash = false;
        pressingDownJump = false;
        releasingJump = false;

        

    }
    
    public void SetGravity()
    {
        if (isJumping)
        {
            rb.gravityScale = JumpingGravityCoefficient * defaultGravityScale;
        }
        else if (isDashing)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = defaultGravityScale;
        }
    }

    public void LandingOnGround()
    {
        isJumping = false;
        DashAvailable = true;
        DoubleJumpAvailable = true;
    }
    
    public IEnumerator ResetOnWall(string ValueParam) //imprécis btw, en pratique c est random entre deux et trois frames
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
    

    public void CancelDash()
    {
        isDashing.Assert(true);
        isDashing = false;

        if ((rb.velocity.x > 0 && preDashVelocity.x > rb.velocity.x) || (rb.velocity.x < 0 && preDashVelocity.x < rb.velocity.x))
        {
            rb.velocity = new Vector2(preDashVelocity.x, rb.velocity.y);
        }

        preDashVelocity = new Vector2(float.NaN, float.NaN);
        timeStartDash = float.NaN;

        if (onGround)
        {
            DashAvailable = true;
        }
    }

    // Les deux coroutine suivante nous ont servies pendant les phases de test à bien vérifier l'état du personnage ( quand il est entrain de walljump ou de dash )
    /*public IEnumerator ColorChangeWallJump()
    {
        PlayerSR.color = Color.black;
        yield return new WaitForSeconds(0.5f);
        PlayerSR.color = Color.white;
    }
    public IEnumerator ColorChangeDash() 
    { 
        PlayerSR.color = Color.yellow;
        yield return new WaitForSeconds(0.5f);
        PlayerSR.color = Color.white;
    }*/
    public IEnumerator TimerInterdictionLeft() //wait for seconds super cringe
    {
        yield return new WaitForSeconds(TimerInterdiction);
        forcedRightMovement = false;
    }
    public IEnumerator TimerInterdictionRight()
    {
        yield return new WaitForSeconds(TimerInterdiction);
        forcedLeftMovement = false;
    }
    public IEnumerator CornerBoostReset()
    {
        PlayerSR.color = Color.red;
        yield return new WaitForSeconds(CornerBoostDuration);
        PlayerSR.color = Color.white;
        inCornerBoostState = false;
    }
    
    Vector2 preDashVelocity = new Vector2(float.NaN, float.NaN);
    float timeStartDash = float.NaN;
    public void OnDash()
    {
        isDashing = true;
        preDashVelocity = rb.velocity;
        timeStartDash = Time.time;

        if (isJumping)
        {
            isJumping = false;
        }
        
        
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
        
        animator.SetBool("isDashing", isDashing);

        rb.velocity = new Vector2(DashAngle.x * DashSpeed, DashAngle.y * DashSpeed);
        DashAvailable = false;

    }

    public bool debugActivateCornerBoost;
    public void OnTriggerExit2D(Collider2D other)
    {
        if (debugActivateCornerBoost && other.gameObject.CompareTag("Corner") && !inCornerBoostState && (isDashing || isJumping) &&!onGround)
        {
            StartCoroutine(CornerBoostReset());            
            inCornerBoostState = true;
        }
    }  
}
