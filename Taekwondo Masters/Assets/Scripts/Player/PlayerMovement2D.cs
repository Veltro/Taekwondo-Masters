using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator _animator;
    private Vector2 _movementInput;
    private Rigidbody2D _rb;
    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference attack;

    void Start()
    {
        // Certifique-se que o Rigidbody2D está anexado ao objeto Player
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _movementInput = move.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // Usar Rigidbody2D para movimento
        _rb.velocity = new Vector2(_movementInput.x * moveSpeed, _movementInput.y * moveSpeed);

        // Atualizar animação com base no movimento
        if (_movementInput.magnitude > 0)
        {
            Debug.Log("Movimento iniciado!");
            _animator.SetTrigger("isRunning");
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isIdle", false);
        }
        else
        {
            Debug.Log("Movimento parado!");
            _animator.ResetTrigger("isRunning");
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isIdle", true);
        }
    }

    public void OnEnable()
    {
        attack.action.started += Attack;
        jump.action.started += Jump;
    }

    private void OnDisable()
    {
        attack.action.started -= Attack;
        jump.action.started -= Jump;
    }
    public void Move(InputAction.CallbackContext context)
    {
        // Capturar input de movimento
        _movementInput = context.ReadValue<Vector2>();
        Debug.Log("Movimento realizado!");
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Pulo realizado!");
            _animator.SetTrigger("isJumping"); // Usar Trigger para animação de ataque
            _animator.SetBool("isJumping", true);
            _animator.SetBool("isIdle", false);
        }
        else
        {
            Debug.Log("Pulo parado!");
            _animator.ResetTrigger("isJumping");
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isIdle", true);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Ataque realizado!");
            _animator.SetTrigger("isAttacking"); // Usar Trigger para animação de ataque
            _animator.SetBool("isAttacking", true);
            _animator.SetBool("isIdle", false);
        }
        else
        {
            Debug.Log("Ataque parado!");
            _animator.ResetTrigger("isAttacking");
            _animator.SetBool("isAttacking", false);
            _animator.SetBool("isIdle", true);
        }
    }
}