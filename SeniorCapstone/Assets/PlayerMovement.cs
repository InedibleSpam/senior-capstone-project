using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class RealisticSpaceMovement : MonoBehaviour
{
    [Header("Thruster Settings")]
    public float thrustAcceleration = 10f; // How fast you gain speed
    public float maxSpeed = 5f;
    
    [Header("Physics (The 'Feel')")]
    [Range(0.1f, 10f)]
    public float drag = 2f;            // How fast you naturally slow down
    public float rotationSmoothing = 5f; // Higher = Snappier, Lower = Floatier

    [Header("References")]
    public InputActionAsset inputActions;
    public Transform cameraTransform;

    private CharacterController controller;
    private InputActionMap playerActionMap;
    
    private Vector2 moveInput;
    private Vector2 rotateInput;
    private Vector3 worldVelocity;
    private float rotationVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerActionMap = inputActions.FindActionMap("Player");
        playerActionMap.Enable();

        playerActionMap.FindAction("Move").performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerActionMap.FindAction("Move").canceled += ctx => moveInput = Vector2.zero;

        playerActionMap.FindAction("VerticalMove").performed += ctx => rotateInput = ctx.ReadValue<Vector2>();
        playerActionMap.FindAction("VerticalMove").canceled += ctx => rotateInput = Vector2.zero;
    }

    void Update()
    {
        HandleRotation();
        HandlePhysicsMovement();
    }

    void HandleRotation()
    {
        // Smooth rotation that feels like rotating a heavy object
        float targetRotSpeed = rotateInput.x * 60f; 
        rotationVelocity = Mathf.Lerp(rotationVelocity, targetRotSpeed, Time.deltaTime * rotationSmoothing);
        transform.Rotate(0, rotationVelocity * Time.deltaTime, 0);
    }

    void HandlePhysicsMovement()
    {
        // 1. Get input direction relative to where you are looking
        Vector3 inputDir = (cameraTransform.right * moveInput.x) + (cameraTransform.forward * moveInput.y);
        
        // Include vertical drift if you want the right stick Y to control Up/Down
        inputDir += transform.up * rotateInput.y;

        // 2. Apply Acceleration
        if (inputDir.magnitude > 0.1f)
        {
            // Pushing the stick adds velocity
            worldVelocity += inputDir * thrustAcceleration * Time.deltaTime;
        }

        // 3. Apply Drag (The "Exponential Decay")
        // This is the secret sauce: it reduces velocity by a percentage every frame
        // rather than a fixed amount, leading to a smooth, realistic stop.
        worldVelocity = Vector3.Lerp(worldVelocity, Vector3.zero, drag * Time.deltaTime);

        // 4. Clamp speed so you don't become a rocket
        worldVelocity = Vector3.ClampMagnitude(worldVelocity, maxSpeed);

        // 5. Final Move
        controller.Move(worldVelocity * Time.deltaTime);
    }
}