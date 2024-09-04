using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    GameInput _gameInput1, _gameInput2;

    InputDevice _player1Device, _player2Device;

    private void OnEnable()
    {
        if (_gameInput1 == null && _gameInput2 == null)
        {
            _gameInput1 = new GameInput();
            _gameInput2 = new GameInput();

            _gameInput1.Gameplay.SetCallbacks(this);
            _gameInput2.Gameplay.SetCallbacks(this);

            SetGameplay();
        }
    }

    public void SetGameplay()
    {
        _gameInput1.Gameplay.Enable();
        _gameInput2.Gameplay.Enable();
    }

    private void AssignPlayerDevice(InputDevice device)
    {
        if (_player1Device == null)
        {
            _player1Device = device;
            _gameInput1.devices = new[] { _player1Device };
            Debug.Log("Asignado Jugador 1: " + _player1Device.name);
        }
        else if (_player2Device == null && device != _player1Device)
        {
            _player2Device = device;
            _gameInput2.devices = new[] { _player2Device };
            Debug.Log("Asignado Jugador 2: " + _player2Device.name);
        }
        else if (device != _player1Device && device != _player2Device)
        {
            Debug.Log("No hay espacio para más jugadores");
        }

    }

    public event Action<Vector2, int> MoveEvent;

    public event Action<int> JumpStartEvent;
    public event Action<int> JumpEvent;
    public event Action<int> JumpCancelEvent;

    public event Action<int> AttackStartEvent;
    public event Action<int> AttackCancelEvent;

    public void OnJump(InputAction.CallbackContext context)
    {
        AssignPlayerDevice(context.control.device);

        if (context.control.device == _player1Device)
            HandleJumpEvents(context, 1);
        
        if (context.control.device == _player2Device)
            HandleJumpEvents(context, 2);
    }

    void HandleJumpEvents(InputAction.CallbackContext context, int playerID)
    {
        if (context.phase == InputActionPhase.Started) JumpStartEvent?.Invoke(playerID);
        if (context.phase == InputActionPhase.Performed) JumpEvent?.Invoke(playerID);
        if (context.phase == InputActionPhase.Canceled) JumpCancelEvent?.Invoke(playerID);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        AssignPlayerDevice(context.control.device);

        if (context.control.device == _player1Device)
            MoveEvent?.Invoke(context.ReadValue<Vector2>(), 1);
        
        if (context.control.device == _player2Device)
            MoveEvent?.Invoke(context.ReadValue<Vector2>(), 2);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        AssignPlayerDevice(context.control.device);

        if (context.control.device == _player1Device)
            HandleAttackEvents(context, 1);
        
        if (context.control.device == _player2Device)
            HandleAttackEvents(context, 2);
    }

    private void HandleAttackEvents(InputAction.CallbackContext context, int playerId)
    {
        if (context.phase == InputActionPhase.Started) AttackStartEvent?.Invoke(playerId);
        if (context.phase == InputActionPhase.Canceled) AttackCancelEvent?.Invoke(playerId);
    }
}
