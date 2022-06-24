using UnityEngine;
using UnityEngine.InputSystem;

public class FPInputs : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool SprintButtonPressed { get; private set; }
    public bool JumpButtonPressed { get; private set; }

    private InputAssets _inputAssets;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;
    //test

    private void Awake()
    {
        _inputAssets = new InputAssets();
        _moveAction = _inputAssets.Player.Move;
        _lookAction = _inputAssets.Player.Look;
        _jumpAction = _inputAssets.Player.Jump;
        _sprintAction = _inputAssets.Player.Sprint;
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _lookAction.Enable();
        _jumpAction.Enable();
        _sprintAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _lookAction.Disable();
        _jumpAction.Disable();
        _sprintAction.Disable();
    }

    private void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        LookInput = _lookAction.ReadValue<Vector2>();
        JumpButtonPressed = _jumpAction.WasPressedThisFrame();
        SprintButtonPressed = _sprintAction.WasPressedThisFrame();
    }
}
