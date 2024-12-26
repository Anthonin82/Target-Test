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
            Manager.managerInstance.OnKeyPressed();
        }
    }
    public void OnLeftButton(InputAction.CallbackContext context)
    {
        
        if (context.started)
        {
            movement.pressingLeft = true;
            Manager.managerInstance.OnKeyPressed();
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
            Manager.managerInstance.OnKeyPressed();
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
            Manager.managerInstance.OnKeyPressed();
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
            Manager.managerInstance.OnKeyPressed();
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
            Manager.managerInstance.pauseMenu.Retry();
        }
    }







}
