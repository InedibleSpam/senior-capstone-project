using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class RealisticSpaceMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float thrustAcceleration = 8f; // How fast you pick up speed
    public float maxSpeed = 4f;           // Cap speed for VR comfort
    [Range(0.1f, 10f)]
    public float drag = 1.5f;             // How fast you naturally drift to a stop

    [Header("Impact (Metal Thud)")]
    public AudioClip impactSound;         // Assign "Metal Thud" here
    public float minImpactSpeed = 0.5f;   // Minimum speed to trigger a sound
    [Range(0f, 1f)]
    public float impactVolume = 0.6f;

    [Header("References")]
    public InputActionAsset inputActions;
    public Transform cameraTransform;

    private CharacterController controller;
    private AudioSource impactSource;
    private Vector2 moveInput;
    private Vector2 rotateInput;
    private Vector3 worldVelocity;
    private float rotationVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Setup the "Thud" Speaker automatically
        impactSource = gameObject.AddComponent<AudioSource>();
        impactSource.playOnAwake = false;
        impactSource.spatialBlend = 0f; // 2D sounds like it's "your" body/helmet hitting

        // Setup Input Actions
        var actionMap = inputActions.FindActionMap("Player");
        if (actionMap != null)
        {
            actionMap.Enable();
            actionMap.FindAction("Move").performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            actionMap.FindAction("Move").canceled += ctx => moveInput = Vector2.zero;
            
            // Right stick X = Rotate, Y = Vertical Thrust
            actionMap.FindAction("VerticalMove").performed += ctx => rotateInput = ctx.ReadValue<Vector2>();
            actionMap.FindAction("VerticalMove").canceled += ctx => rotateInput = Vector2.zero;
        }
    }

    void Update()
    {
        HandleRotation();
        HandlePhysicsMovement();
    }

    void HandleRotation()
    {
        // Smooth rotation momentum
        float targetRotSpeed = rotateInput.x * 60f; 
        rotationVelocity = Mathf.Lerp(rotationVelocity, targetRotSpeed, Time.deltaTime * 4f);
        transform.Rotate(0, rotationVelocity * Time.deltaTime, 0);
    }

    void HandlePhysicsMovement()
    {
        // 1. Calculate direction based on where you are looking
        Vector3 inputDir = (cameraTransform.right * moveInput.x) + (cameraTransform.forward * moveInput.y);
        
        // Use Right Stick Y for manual Up/Down thrust
        inputDir += transform.up * rotateInput.y;

        // 2. Apply Thrust
        if (inputDir.magnitude > 0.1f)
        {
            worldVelocity += inputDir * thrustAcceleration * Time.deltaTime;
        }

        // 3. Apply Drift Drag (Exponential Decay)
        worldVelocity = Vector3.Lerp(worldVelocity, Vector3.zero, drag * Time.deltaTime);

        // 4. Clamp to Max Speed
        worldVelocity = Vector3.ClampMagnitude(worldVelocity, maxSpeed);

        // 5. Execute Movement
        controller.Move(worldVelocity * Time.deltaTime);
    }

    // Fires when the CharacterController hits a wall, ceiling, or floor
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float speedAtImpact = worldVelocity.magnitude;

        if (speedAtImpact > minImpactSpeed && !impactSource.isPlaying)
        {
            // Subtle pitch randomization so it's not identical every time
            impactSource.pitch = Random.Range(0.9f, 1.1f);
            
            // Scale volume by how fast you were going
            float volume = Mathf.Clamp01(speedAtImpact / maxSpeed) * impactVolume;
            impactSource.PlayOneShot(impactSound, volume);
            
            // PHYSICS: Lose 40% of your speed when you hit something (Inertia)
            worldVelocity *= 0.6f; 
        }
    }
}