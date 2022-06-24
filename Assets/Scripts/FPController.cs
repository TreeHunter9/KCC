using System;
using PlayerStates;
using UnityEngine;

[RequireComponent(typeof(FPInputs), typeof(CharacterController))]
public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 60f;
    public float accelerationTime = 0.2f;
    public float sprintSpeed = 90f;
    public float sprintAccelerationTime = 0.5f;
    
    [Header("Jump Settings")]
    public float jumpHeight = 1f;
    public float jumpMultiplierAcceleration = 1.1f;
    public float maxJumpPenalty = 0.2f;
    public float timeToJumpRecovery = 1f;
    public float maxAdditionalVelocityToInputs = 0.1f;
    
    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    public float airDrag = 0.01f;
    public float rotationSpeed = 1f;

    [Header("Camera Settings")]
    [SerializeField] private Transform _cameraRootTransform;


    [SerializeField] private float _sensitivity = 3f;
    [SerializeField] private float _maxVerticalAngle = 89f;
    [SerializeField] private float _minVerticalAngle = -89f;

    public BaseState GroundedState { get; private set; }
    public BaseState FlayingState { get; private set; }
    public BaseState currentState;
    public CharacterController CharacterControllerComponent => _characterControllerComponent;
    public FPInputs FpInputs => _fpInputs;
    public bool isSprinting = false; 

    private CharacterController _characterControllerComponent;

    private FPInputs _fpInputs;
    private Vector2 _smoothInputs;

    [HideInInspector] public Vector2 smoothInput;
    [HideInInspector] public Vector3 smoothDampVelocity;
    [HideInInspector] public Vector3 currentVelocity;

    private void Awake()
    {
        _characterControllerComponent = GetComponent<CharacterController>();
        _fpInputs = GetComponent<FPInputs>();
        
        GroundedState = new GroundedState(this);
        FlayingState = new FlyingState(this);
        currentState = GroundedState;
        currentState.EnterState();
    }

    private void Update()
    {
        UpdateIsSprintingValue();
        currentState.UpdateStates();
        Rotation();
        //Debug.Log(GetRawPlaneVelocity(transform.localRotation).magnitude);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float angle = Vector3.Angle(Vector3.up, hit.normal);
        if (angle < _characterControllerComponent.slopeLimit)
            return;
        float dotProduct = Vector3.Dot(new Vector3(currentVelocity.x, 0, currentVelocity.z).normalized, hit.normal.normalized);
        if (dotProduct < -0.1f)
        {
            float penalty = Mathf.Lerp(0.8f, 0.9999f, Mathf.Pow(1 + dotProduct, 0.2f));
            currentVelocity.x *= penalty;
            currentVelocity.z *= penalty;
        }
    }

    public Vector3 GetRawVelocity(Quaternion currentRotation) => Quaternion.Inverse(currentRotation) * currentVelocity;
    
    public Vector2 GetRawPlaneVelocity(Quaternion currentRotation)
    {
        Vector3 rawVelocity = GetRawVelocity(currentRotation);
        return new Vector2(rawVelocity.x, rawVelocity.z);
    }

    public bool IsGrounded(Transform trans, CharacterController characterController, out RaycastHit hitInfo)
    {
        Vector3 position = trans.position;
        Vector3 spherePosition = new Vector3(position.x, position.y + characterController.radius,
            position.z);
        return Physics.SphereCast(spherePosition, characterController.radius, Vector3.down, out hitInfo, 0.025f, ~(1 << 8),
            QueryTriggerInteraction.Ignore);
    }

    private void UpdateIsSprintingValue()
    {
        if (_fpInputs.SprintButtonPressed)
        {
            isSprinting = true;
        }

        if (isSprinting == true && _fpInputs.MoveInput.y <= 0)
        {
            isSprinting = false;
        }
    }

    private void Rotation()
    {
        Vector3 cameraAngles = _cameraRootTransform.localRotation.eulerAngles;
        cameraAngles.x = ClampAngle(cameraAngles.x + _fpInputs.LookInput.y * _sensitivity, _minVerticalAngle, _maxVerticalAngle);
        _cameraRootTransform.localRotation = Quaternion.Euler(cameraAngles.x, 0f, 0f);
        transform.localRotation *= Quaternion.Euler(0f, _fpInputs.LookInput.x * _sensitivity, 0f);
    }

    private float ClampAngle(float value, float min, float max)
    {
        if (value > 180)
            value -= 360;
        return Mathf.Clamp(value, min, max);
    }

    private Vector3 p1;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(p1, 0.5f);
    }
}
