using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private Rigidbody rb;
    private InputAction moveInput, jumpInput, crouchInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        moveInput = InputSystem.actions.FindAction("Move");
        jumpInput = InputSystem.actions.FindAction("Jump");
        crouchInput = InputSystem.actions.FindAction("Crouch");
    }
    private void Update()
    {
        MovePlayer(moveInput.ReadValue<Vector2>(), jumpInput.triggered);
        crouchInput.started += OnCrouchStarted;
        crouchInput.canceled += OnCrouchCancelled;
    }
    public void GetAnim(Animator pAnim)
    {
        anim = pAnim;
    }
    private void MovePlayer(Vector2 move, bool jump)
    {

        Move(move);
        Jump(jump);
    }

    private void Move(Vector2 move)
    {
        anim.SetFloat("Horizontal", move.x, 0.2f, Time.deltaTime);
        anim.SetFloat("Vertical", move.y, 0.2f, Time.deltaTime);
    }
    [SerializeField] float jumpForce;
    private void Jump(bool isJumping)
    {

        if (isJumping)
        {
            anim.ResetTrigger("Jump");
            anim.SetTrigger("Jump");
            var direction = new Vector3(moveInput.ReadValue<Vector2>().x, 1, moveInput.ReadValue<Vector2>().y).normalized;
            Vector3 jumpForceVector = Vector3.up * jumpForce;

            if (direction.magnitude > 0.01f)
            {
                jumpForceVector += direction * jumpForce;
            }

        }

    }
    private void OnCrouchStarted(InputAction.CallbackContext ctx)
    {
        anim.SetBool("Crouching", true);
    }
    private void OnCrouchCancelled(InputAction.CallbackContext ctx)
    {
        anim.SetBool("Crouching", false);
    }

}