using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    public float pitchClamp = 80f;
    public bool isGrounded;
    public bool isAttacking;
    [SerializeField] private Animator _animator;


    CharacterController controller;
    Vector3 velocity;
    float pitch;

    PlayerInput input;
    InputAction move;
    InputAction look;
    InputAction jump;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        move = input.actions["Move"];
        look = input.actions["Look"];
        jump = input.actions["Jump"];
        isAttacking = false;
        
        // Ensure all Move() calls check collisions, even for tiny distances
        controller.minMoveDistance = 0f;
    }

    void Update()
    {
        // Horizontal movement first
        Vector2 moveInput = move.ReadValue<Vector2>();
        Vector3 moveVec = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(moveVec * moveSpeed * Time.deltaTime);

        // Jump check (uses grounded state after horizontal move; fine for most cases)
        if (jump.triggered) {
            if(isGrounded){
                _animator.SetBool("isJumping", true);
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // control the jump and then fall animation
        if(velocity.y < 0){
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isGrounded", false);
            _animator.SetBool("isFalling", true);
        } 

        // resume animation after jump landed
        if(isGrounded) {
            _animator.SetBool("isGrounded", true);
            _animator.SetBool("isFalling", false);           
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        
        // Vertical movement
        controller.Move(velocity * Time.deltaTime);
        
        // Now read isGrounded after the full movement (reflects latest collision)
        isGrounded = controller.isGrounded;
        
        // Reset downward velocity if grounded (for next frame's sticking)
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        // Mouse look
        Vector2 lookInput = look.ReadValue<Vector2>() * mouseSensitivity;
        pitch = Mathf.Clamp(pitch - lookInput.y, -pitchClamp, pitchClamp);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
        transform.Rotate(Vector3.up * lookInput.x);

        if (moveInput.sqrMagnitude > 0.01f) {
            _animator.SetBool("isRunning", true);
        } else {
            _animator.SetBool("isRunning", false);

        }

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            _animator.SetTrigger("Punch");
            isAttacking = true;
            StartCoroutine(ResetAttackAfterDelay(1f)); // Call coroutine
        }

    }
    
    private IEnumerator ResetAttackAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }
}