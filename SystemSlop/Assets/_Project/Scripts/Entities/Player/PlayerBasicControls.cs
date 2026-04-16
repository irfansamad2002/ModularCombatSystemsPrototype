using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.InputSystem;

public class PlayerBasicControls : MonoBehaviour
{
    public InputActionAsset InputActions;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float acceleration = 5f;

    [Header("Jump & Gravity")]
    [SerializeField] private float gravity = -15f;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float terminalVelocity = 53f;

    [Header("Ground Check")]
    [SerializeField] private float groundedRadius = .5f;
    [SerializeField] private float groundedOffset = -0.14f;
    [SerializeField] private LayerMask groundLayers;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    
    private Vector2 _moveAmt;
    private float _speed;
    private float _verticalVelocity;

    private bool _isGrounded;
    private bool _jumpRequested;

    private float _jumpTimeout = .1f;
    private float _jumpTimeoutDelta;

    private float _fallTimeout = .15f;
    private float _fallTimeoutDelta;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        var map = InputActions.FindActionMap("Player");

        _moveAction = map.FindAction("Move");
        _jumpAction = map.FindAction("Jump");
    }

    private void Start()
    {
        _jumpTimeoutDelta = _jumpTimeout;
    }

    private void Update()
    {
        ReadInput();
        GroundedCheck();
        HandleMovement();
        HandleGravityAndJump();
    }

    private void ReadInput()
    {
        _moveAmt = _moveAction.ReadValue<Vector2>();

        if (_jumpAction.WasPressedThisFrame())
        {
            Debug.Log("Jump");// Placeholder
            _jumpRequested = true;
        }
    }
  
    private void HandleMovement()
    {
        float targetSpeed = movementSpeed;

        if(_moveAmt == Vector2.zero) targetSpeed = 0f;

        float currentSpeed = GetCurrentHorizontalSpeed();
        _speed = GetSmoothedSpeed(currentSpeed, targetSpeed);

        Vector3 moveDirection = GetMoveDirection();

        Vector3 finalVelocity = moveDirection * _speed;
        finalVelocity.y = _verticalVelocity;

        characterController.Move(finalVelocity * Time.deltaTime);
        
    }
    private float GetCurrentHorizontalSpeed()
    {
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z);

        return horizontalVelocity.magnitude;
    }
    private float GetSmoothedSpeed(float current, float target)
    {
        float offset = .1f;

        if (Mathf.Abs(current - target) > offset)
        {
            float smoothed = Mathf.Lerp(current, target, Time.deltaTime * acceleration);
            return Mathf.Round(smoothed * 1000f) / 1000f;
        }

        return target;
    }
    private Vector3 GetMoveDirection()
    {
        if (_moveAmt == Vector2.zero) return Vector3.zero;

        // Camera forward/rigth flatted to ground
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = right * _moveAmt.x + forward * _moveAmt.y;

        return direction.normalized;
    }

    // --------------- GRAVITY & JUMP ---------------
    private void GroundedCheck()
    {
        Vector3 spherePos = new Vector3(transform.position.x,
                                        transform.position.y + groundedOffset,
                                        transform.position.z);

        _isGrounded = Physics.CheckSphere(
            spherePos, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }
    private void HandleGravityAndJump()
    {
        HandleGroundedState();
        HandleJumpRequest();
        ApplyGravity();
       
    }
    private void HandleGroundedState()
    {
        if (_isGrounded)
        {
            _fallTimeoutDelta = _fallTimeout;

            // Stick to the ground (prevent floaty behaviour)
            if (_verticalVelocity < 0f) _verticalVelocity = -2f;
        }
        else
        {

            _jumpTimeoutDelta = _jumpTimeout;
        }
    }
    private void HandleJumpRequest()
    {
        if (!_isGrounded) return;

        if (_jumpTimeoutDelta > 0f)
        {
            _jumpTimeoutDelta -= Time.deltaTime;
            return;
        }
        if (!_jumpRequested) return;

        //EXECUTE JUMP
        _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        _jumpRequested = false;
        _jumpTimeoutDelta = _jumpTimeout;
    }
    private void ApplyGravity()
    {
        if (_verticalVelocity < terminalVelocity)
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = _isGrounded ? Color.yellow : Color.red;

        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z);

        Gizmos.DrawWireSphere(spherePos, groundedRadius);
    }
}

