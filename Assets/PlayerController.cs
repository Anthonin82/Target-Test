using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float horizontalSpeed;

    public LayerMask wallLayer;
    public float wallDetectionHorizontalDistance = 0.1f;
    public float wallDetectionVerticalDistance = 1f;

    public bool onLeftWall;
    public bool onRightWall;
    public bool onWall;

    public bool onGround;   

    public bool isJumping = false;
    public bool reachedJumpApexThisFrame = false;

    public Transform leftPlayerSide;
    public Transform rightPlayerSide;
    public Transform bottomPlayerSide;

    bool pressingLeft;
    bool pressingRight;
    bool pressingUp;
    bool pressingDown;

    bool pressingJump;
    
    public bool pressingDash;
    public float DashLength;
    public Vector2 DashAngle;
    public float DashTime;
    


    private void Update()
    {
        pressingLeft = Input.GetKey(KeyCode.LeftArrow);
        pressingRight = Input.GetKey(KeyCode.RightArrow);
        pressingUp = Input.GetKey(KeyCode.UpArrow);
        pressingDown = Input.GetKey(KeyCode.DownArrow);

        pressingJump = Input.GetKey(KeyCode.Z);
        pressingDash = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyUp(KeyCode.Z) && isJumping)
        {
            isJumping = false;
            reachedJumpApexThisFrame = true;
        }
    }


    private void FixedUpdate()
    {

        onLeftWall = Physics2D.BoxCast(leftPlayerSide.position, new Vector2(wallDetectionHorizontalDistance, wallDetectionVerticalDistance), 0, Vector2.left, 0, wallLayer);
        onRightWall = Physics2D.BoxCast(rightPlayerSide.position, new Vector2(wallDetectionHorizontalDistance, wallDetectionVerticalDistance), 0, Vector2.right, 0, wallLayer);
        onWall = onLeftWall || onRightWall;
        onGround = Physics2D.Raycast(bottomPlayerSide.position, Vector2.down, 0.01f, wallLayer);   

        if (pressingLeft && !pressingRight && !onLeftWall)
        {
            rb.velocity = new Vector2(-horizontalSpeed, rb.velocity.y);
        }
        else if (pressingRight && !pressingLeft && !onRightWall)
        {
            rb.velocity = new Vector2(horizontalSpeed, rb.velocity.y);
        }
        else if (!pressingLeft && !pressingRight)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if(pressingJump && onGround && !isJumping)// could be onGround and jumping, since the onGround check could be a bit too wide
        {
            isJumping = true;

        }
        
        

        if (pressingDash && !onWall)
        {
            DashAngle.x = Input.GetAxis("Horizontal");
            DashAngle.y = Input.GetAxis("Vertical");
            Debug.Log(DashAngle);
            
            rb.velocity = new Vector2(DashAngle.x * DashLength, DashAngle.y * DashLength);
            StartCoroutine("DashTimer");
            
        }

    }
    public IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(DashTime);
        rb.velocity = Vector2.zero;

    }




}
