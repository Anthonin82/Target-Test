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

    public Transform leftPlayerSide;
    public Transform rightPlayerSide;


    private void FixedUpdate()
    {

        onLeftWall = Physics2D.BoxCast(leftPlayerSide.position, new Vector2(wallDetectionHorizontalDistance, wallDetectionVerticalDistance), 0, Vector2.left, 0, wallLayer);
        onRightWall = Physics2D.BoxCast(rightPlayerSide.position, new Vector2(wallDetectionHorizontalDistance, wallDetectionVerticalDistance), 0, Vector2.right, 0, wallLayer);
        onWall = onLeftWall || onRightWall;

        bool pressingLeft = Input.GetKey(KeyCode.LeftArrow);
        bool pressingRight = Input.GetKey(KeyCode.RightArrow);
        bool pressingUp = Input.GetKey(KeyCode.UpArrow);
        bool pressingDown = Input.GetKey(KeyCode.DownArrow);

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


    }




}
