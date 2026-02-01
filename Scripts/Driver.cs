using UnityEngine;
using UnityEngine.InputSystem;

public class Driver: MonoBehaviour
{
    [Header("Car Settings")]
    public float accelerationFactor = 12f;
    public float maxSpeed = 10f;
    public float turnFactor = 3f;
    public float driftFactor = 0.92f;
        
    // Local Variables
    float accelerationInput = 0;
    float steerInput = 0f;
    float rotationAngle = 0f;
    float velocityisUP;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rotationAngle = 90f;
        rb.rotation = 90f;
    }
    void Update()
    {
        accelerationInput = (Keyboard.current.wKey.isPressed ? 1f : 0f) + (Keyboard.current.sKey.isPressed ? -1f : 0f);
        steerInput = (Keyboard.current.aKey.isPressed ? -1f : 0f) + (Keyboard.current.dKey.isPressed ? 1f : 0f);
    }
    void FixedUpdate()
    {
        ApplyEngine();
        killOrthogonalVelocity();
        ApplySteering();
    }

    void ApplyEngine()
    {
        velocityisUP = Vector2.Dot(rb.linearVelocity, transform.up);
        if(velocityisUP > maxSpeed && accelerationInput > 0)
        {
            return;
        }
        if(velocityisUP < -maxSpeed * 0.5f && accelerationInput > 0)
        {
            return;
        }
        if (accelerationInput == 0)
        {
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, 3f, Time.fixedDeltaTime * 3f);
        }
        else
        {
            rb.linearDamping = 0f;
        }
        Vector2 engineForce = transform.up * accelerationFactor * accelerationInput;
        rb.AddForce(engineForce, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        float speed = rb.linearVelocity.magnitude;
        float direction = Mathf.Sign(Vector2.Dot(rb.linearVelocity, transform.up));
        float minSpeedBeforeAllowTurnFactor = speed / 2f;

        minSpeedBeforeAllowTurnFactor = Mathf.Clamp01(minSpeedBeforeAllowTurnFactor);
        rotationAngle -= steerInput * turnFactor * minSpeedBeforeAllowTurnFactor * direction;

        rb.MoveRotation(rotationAngle); 
    }

    void killOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
 
        rb.linearVelocity = forwardVelocity + (rightVelocity * driftFactor);
    }
}