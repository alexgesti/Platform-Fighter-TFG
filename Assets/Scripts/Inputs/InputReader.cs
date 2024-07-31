using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    GameInput _gameInput;
    
    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
    
            _gameInput.Gameplay.SetCallbacks(instance: this);
    
            SetGameplay();
        }
    }
    
    public void SetGameplay()
    {
        _gameInput.Gameplay.Enable();
    }

    public event Action<Vector2> MoveEvent;

    public event Action JumpStartEvent;
    public event Action JumpEvent;
    public event Action JumpCancelEvent;

    public event Action AttackStartEvent;
    public event Action AttackCancelEvent;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) JumpStartEvent?.Invoke();
        if (context.phase == InputActionPhase.Performed) JumpEvent?.Invoke();
        if (context.phase == InputActionPhase.Canceled) JumpCancelEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(obj: context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) AttackStartEvent?.Invoke();
        if (context.phase == InputActionPhase.Canceled) AttackCancelEvent?.Invoke();
    }
}
