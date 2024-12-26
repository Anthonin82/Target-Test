using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerMovement movement;




    BoolWrapper wentThroughFixedUpdateSinceButtonPressed = new(false);

    //exception 1 : si on part d une touche relachée, qu'on press et qu on relache avant fixed update,
    //on veut quand même une frame ou la touche est press.
    //exception 2 : si on part d une touche pressée, qu'on lache et qu on represse avant fixed update,
    //on veut quand même une frame ou la touche est relachée.
    //WARNING : cette expection 2 ne doit etre valable que pour le saut. 
    //TODO : faire avec un event certainement, avec les coroutines nsm

    public void OnJumpButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            movement.pressingJump = true;
        }

        if (context.canceled)
        {
            movement.pressingJump = false;
        }
    }







    //dehors

    public void SetAtEndNextFixedUpdate(BoolWrapper boolWrapper, bool futureValue)
    {
        
        StartCoroutine(SetFalseAtEndNextFixedUpdateCoroutine(boolWrapper, futureValue));
    }

    public IEnumerator SetFalseAtEndNextFixedUpdateCoroutine(BoolWrapper wrapperValue, bool futureValue)
    {
        yield return new WaitForFixedUpdate();
        (wrapperValue.value == futureValue).Assert(false);
        wrapperValue.value = futureValue;
    }

    public class BoolWrapper
    {
        public BoolWrapper(bool initialValue)
        {
            value = initialValue;
        }

        public bool value;
    }

}
