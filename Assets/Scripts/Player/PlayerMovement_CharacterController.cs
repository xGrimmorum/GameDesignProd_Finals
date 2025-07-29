using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement_CharacterController : MonoBehaviour
{
    [SerializeField] private GrapplingHook grapplingHook;
    private CharacterController controller;
    private InputAction moveInput, sprintInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();

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

    public bool PullPlayerTowardGrapple(Vector3 grappleTarget, float grappleSpeed, float climbDistance, LineRenderer lineRenderer)
    {
        Vector3 dir = (grappleTarget - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, grappleTarget);

        controller.Move(dir * grappleSpeed);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, grappleTarget);

        if (dist < climbDistance)
        {

            Vector3 offset = new Vector3(0, 1.5f, 0);
            transform.position = grappleTarget + offset;

            if(controller.isGrounded) controller.Move(Vector3.zero);

            return true;

            // Climb/jump onto roof
        }
        return false;
    }
}
