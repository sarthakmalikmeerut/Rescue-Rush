using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;


public class Driver : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] float accelerationFactor = 15f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float drag = 0.90f;
    [SerializeField] Vector2 currentSpeed;
    [SerializeField] float boostSpeed = 5f;





    [Header("Steering Settings")]
    [SerializeField] float turnFactor = 3.2f;
    [SerializeField] float driftFactor = 0.86f;
    [SerializeField] float steeringSmoothness = 8f;
    [SerializeField] float steeringDrag = 2f;
    
    private Rigidbody2D rb;
    private float engineInput;
    private float steerInput;
    private float boostInput;
    private float currentMaxSpeed;
    private float targetRotation;
    private float rotationAngle;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        engineInput = (Keyboard.current.wKey.isPressed ? 1f : 0f) + (Keyboard.current.sKey.isPressed ? -1f : 0f);
        steerInput = (Keyboard.current.aKey.isPressed ? -1f : 0f) + (Keyboard.current.dKey.isPressed ? 1f : 0f);
        boostInput = Keyboard.current.shiftKey.isPressed ? 1f : 0f;
    }
    void FixedUpdate()
    {
        handleMovement();
        handleSteering();
        KillLateralVelocity();
    }
    void handleMovement()
    {
        
        currentSpeed = transform.up * engineInput * accelerationFactor;
        
        currentMaxSpeed = maxSpeed;

        if(boostInput != 0f && engineInput > 0f)
        {
            currentMaxSpeed += boostSpeed;
        }


        if(engineInput != 0f)
        {
            rb.AddForce(currentSpeed);
        }
        
        else
        {
            rb.linearVelocity *= drag;
        }

        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, currentMaxSpeed);

        // Log current speed to console
        Debug.Log($"Current Speed: {rb.linearVelocity.magnitude:F2}");
    }
    void handleSteering()
    {
        float velocityMagnitude = rb.linearVelocity.magnitude;
        
        // Gradually reduce steering as velocity decreases
        float steerInfluence = Mathf.Clamp01(velocityMagnitude / steeringDrag); // Adjust 1f as needed

        targetRotation -= steerInput * turnFactor * steerInfluence;

        rotationAngle = Mathf.Lerp(
            rotationAngle,
            targetRotation,
            Time.fixedDeltaTime * steeringSmoothness
        );

        rb.MoveRotation(rotationAngle);
    }
    void KillLateralVelocity()
    {
        // Reduces sideways velocity to prevent infinite drifting, simulating tire friction
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 lateralVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
        
        rb.linearVelocity = forwardVelocity + (lateralVelocity * driftFactor);
    }

}