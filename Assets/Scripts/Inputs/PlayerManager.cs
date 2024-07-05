using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerManager : MonoBehaviour
{
    //public InputReader inputReader;
    //
    //// Start is called before the first frame update
    //void Start()
    //{
    //    var playerInput = GetComponent<PlayerInput>();
    //
    //    playerInput.onActionTriggered += context =>
    //    {
    //        switch (context.action.name)
    //        {
    //            case "Move":
    //                OnMove(context);
    //                break;
    //            case "Jump":
    //                OnJump(context);
    //                break;
    //        }
    //    };
    //
    //    var user = InputUser.PerformPairingWithDevice(playerInput.devices[0]);
    //    user.AssociateActionsWithUser(playerInput);
    //}
    //
    //private void OnMove(InputAction.CallbackContext context)
    //{
    //    Vector2 move = context.ReadValue<Vector2>();
    //    inputReader.MoveEvent?.Invoke(move);
    //}
    //
    //private void OnJump(InputAction.CallbackContext context)
    //{
    //    if (context.phase == InputActionPhase.Started) inputReader.JumpStartEvent?.Invoke();
    //    if (context.phase == InputActionPhase.Performed) inputReader.JumpEvent?.Invoke();
    //    if (context.phase == InputActionPhase.Canceled) inputReader.JumpCancelEvent?.Invoke();
    //}
}
