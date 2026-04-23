using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2D : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 _movementInput;
    private Rigidbody2D _rb;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        _rb.velocity = new Vector2(_movementInput.x * moveSpeed, _rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context) {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if (context.started) {
            Debug.Log("Ataque realizado!"); // Adicione lógica de dano aqui
        }
    }
}