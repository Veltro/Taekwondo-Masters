using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; // Required for Coroutines

public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator _animator;
    private Vector2 _movementInput;
    private Rigidbody2D _rb;
    private bool _isJumpingLocal;
    private bool _isAttackingLocal;

    // New variables for the "Return to Position" mechanic
    private float _startJumpY;
    private bool _isReturningToGround = false;

    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference attack;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _movementInput = move.action.ReadValue<Vector2>();
        bool isMoving = _movementInput.magnitude > 0;
        _animator.SetBool("isRunning", isMoving);
        _animator.SetBool("isIdle", !isMoving && !_isJumpingLocal && !_isAttackingLocal);
    }

    void FixedUpdate()
    {
        float horizontalVelocity = _movementInput.x * moveSpeed;

        // If we are returning to the original position, we let the Coroutine handle Y
        // Otherwise, use physics/input velocity
        float verticalVelocity = _isJumpingLocal ? _rb.linearVelocity.y : _movementInput.y * moveSpeed;

        _rb.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);

        if (_movementInput.magnitude > 0)
        {
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
    }

    public void OnEnable()
    {
        jump.action.performed += OnJump;
        attack.action.started += OnAttack;
    }

    public void OnDisable()
    {
        jump.action.performed -= OnJump;
        attack.action.started -= OnAttack;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        JumpEvent();
        if (!_isJumpingLocal && !_isReturningToGround)
        {
            // STEP 1: Capture the starting Y position before moving
            _startJumpY = transform.position.y;

            _isJumpingLocal = true;
            _animator.SetBool("isJumping", true);

            // Apply upward impulse
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, moveSpeed);

            // Trigger the reset after the jump height phase
            Invoke(nameof(ResetJump), 0.5f);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        AttackEvent();
        if (!_isAttackingLocal)
        {
            _isAttackingLocal = true;
            _animator.SetTrigger("attackTrigger");
            Invoke(nameof(ResetAttack), 0.5f);
        }
    }

    public void JumpEvent() => Debug.Log("Evento de Pular Executado!");
    public void AttackEvent() => Debug.Log("Evento de atacar Executado!");

    private void ResetJump()
    {
        _isJumpingLocal = false;
        _animator.SetBool("isJumping", false);

        // STEP 2: Start the process of moving back to the original height
        StartCoroutine(ReturnToOriginalHeight());
    }

    // STEP 3: The Coroutine that pulls the player back down
    private IEnumerator ReturnToOriginalHeight()
    {
        _isReturningToGround = true;
        float elapsed = 0f;
        float duration = 0.5f; // How long the descent should take

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newY = Mathf.Lerp(transform.position.y, _startJumpY, elapsed / duration);

            // We update the position via Rigidbody to keep physics interactions smooth
            _rb.position = new Vector2(transform.position.x, newY);
            yield return null;
        }

        // Ensure we land exactly at the starting height
        _rb.position = new Vector2(transform.position.x, _startJumpY);
        _isReturningToGround = false;
    }

    private void ResetAttack()
    {
        _isAttackingLocal = false;
        _animator.SetBool("isAttacking", false);
    }
}