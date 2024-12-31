using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerMovement movement;
    public void OnJumpButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            movement.pressingDownJump = true;
        }
        if (context.canceled)
        {
            movement.releasingJump = true;
        }
    }
    public void OnDashButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            movement.pressingDownDash = true;
        }
        if (context.started)
        {
            GameManager.inst.OnKeyPressed();
        }
    }
    public void OnLeftButton(InputAction.CallbackContext context)
    {
        
        if (context.started)
        {
            movement.pressingLeft = true;
            movement.lastHorizontalDirectionPressed = HoritzontalDirection.Left;
            GameManager.inst.OnKeyPressed();
        }
        if (context.canceled)
        {
            movement.pressingLeft = false;
        }
    }
    public void OnRightButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            movement.pressingRight = true;
            movement.lastHorizontalDirectionPressed = HoritzontalDirection.Right;
            GameManager.inst.OnKeyPressed();
        }
        if (context.canceled)
        {
            movement.pressingRight = false;
        }
    }
    public void OnUpButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            movement.pressingUp = true;
            GameManager.inst.OnKeyPressed();
        }
        if (context.canceled)
        {
            movement.pressingUp = false;
        }
    }
    public void OnDownButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            movement.pressingDown = true;
            GameManager.inst.OnKeyPressed();
        }
        if (context.canceled)
        {
            movement.pressingDown = false;
        }
    }

    public void OnRetryButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameManager.inst.pauseMenu.Retry();
        }
    }







}
