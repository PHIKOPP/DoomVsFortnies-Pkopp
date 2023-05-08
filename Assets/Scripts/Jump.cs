using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviour
{
    Rigidbody rigidbody;
    public float jumpStrength = 2;
    public event System.Action Jumped;
    bool _doubleJumped;
    public float dashTime, dashSpeed;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;


    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        // Get rigidbody.
        rigidbody = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {        
        // Jump when the Jump button is pressed and we are on the ground.
        if (Input.GetButtonDown("Jump") && (!groundCheck || groundCheck.isGrounded))
        {
            rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
        }
        if (Input.GetButtonDown("Jump") && !_doubleJumped)
        {
            rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
            _doubleJumped = true;
        }
        if (_doubleJumped && (groundCheck.isGrounded))
            _doubleJumped = false;
        if (Input.GetKeyDown(KeyCode.LeftShift))
            StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        float startTime = Time.time; // need to remember this to know how long to dash
        while (Time.time < startTime + dashTime)
        {
            rigidbody.AddRelativeForce(Vector3.forward * 100 * dashSpeed);
            yield return null;
        }
    }
}
