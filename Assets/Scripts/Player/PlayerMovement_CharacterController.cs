using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement_CharacterController : MonoBehaviour
{
    private CharacterController controller;
    private InputAction moveInput, sprintInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();

        moveInput = InputSystem.actions.FindAction("Move");
        sprintInput = InputSystem.actions.FindAction("Sprint");
    }

    void Update()
    {
        sprintInput.started += OnSprintStarted;
        sprintInput.canceled += OnSprintCancelled;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Horizontal input
        Vector3 move = new Vector3(moveInput.ReadValue<Vector2>().x, 0, moveInput.ReadValue<Vector2>().y);

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        // Jump
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);
    }
    private void OnSprintStarted(InputAction.CallbackContext ctx)
    {
        playerSpeed = 4f;
    }    
    private void OnSprintCancelled(InputAction.CallbackContext ctx)
    {
        playerSpeed = 2f;
    }
}
