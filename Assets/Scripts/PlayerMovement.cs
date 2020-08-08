using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;
    private Animator animator;
    
    private Camera followCam;
    
    public float speed = 6f;
    public float jumpVelocity = 20f;
    [Range(0.01f, 1f)] public float airControlPercent;

    public float speedSmoothTime = 0.1f;
    public float turnSmoothTime = 0.1f;
    
    private float speedSmoothVelocity;
    private float turnSmoothVelocity;
    
    private float currentVelocityY;

    // expression-bodied member
    public float currentSpeed =>
        new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;
    
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        followCam = Camera.main;
    }

    // Physical relevant tasks
    private void FixedUpdate()
    {
        if (currentSpeed > 0.2f || playerInput.fire) Rotate();

        Move(playerInput.moveInput);
        
        if (playerInput.jump) Jump();
    }

    // Animation relevant tasks
    private void Update()
    {
        UpdateAnimation(playerInput.moveInput);
    }

    public void Move(Vector2 moveInput)
    {
        // Speed with damp
        float targetSpeed = speed * moveInput.magnitude;
        float smoothTime = characterController.isGrounded ? (speedSmoothTime) : (speedSmoothTime / airControlPercent);
        targetSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, smoothTime);

        // Velocity
        Vector3 direction = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x);
        currentVelocityY += Time.deltaTime * Physics.gravity.y;
        Vector3 currentVelocity = (direction * targetSpeed) + (Vector3.up * currentVelocityY);

        // Move
        characterController.Move(currentVelocity * Time.deltaTime);

        // 
        if (characterController.isGrounded) currentVelocityY = 0f;
    }

    public void Rotate()
    {
        float targetRotation = followCam.transform.eulerAngles.y;
        targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

        transform.eulerAngles = Vector3.up * targetRotation;
    }

    public void Jump()
    {
        if (!characterController.isGrounded)
        {
            return;
        }

        currentVelocityY = jumpVelocity;
    }

    private void UpdateAnimation(Vector2 moveInput)
    {
        float animationSpeedPercent = currentSpeed / speed;
        animator.SetFloat("Vertical Move", moveInput.y * animationSpeedPercent, 0.05f, Time.deltaTime);
        animator.SetFloat("Horizontal Move", moveInput.x * animationSpeedPercent, 0.05f, Time.deltaTime);
    }
}