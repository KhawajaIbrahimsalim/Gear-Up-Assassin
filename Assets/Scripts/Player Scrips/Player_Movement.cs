using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

class Player_Movement : MonoBehaviour
{
    [Header("Movement Parameters"), SerializeField, Range(0.5f, 5f)]
    private float Speed;
    [SerializeField]
    private float TopSpeed;
    [SerializeField, Range(0.1f, 1f)]
    private float Acceleration;
    [SerializeField, Range(0.01f, 0.5f)]
    private float Deceleration;
    [SerializeField, Range(0.01f, 0.5f)]
    private float DecelerateLimit;//The minimum spped where Deceleration will stop
    [SerializeField]
    private float Jumpforce;
    [SerializeField]
    private GroundChecker GroundCheck;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Transform Ancor;
    [SerializeField]
    private Transform Hands;

    [Header("Camera Parameters"), SerializeField]
    Vector2 Turn;
    [SerializeField, Range(0.1f, 5f)]
    private float Sensitivity;
    [SerializeField]
    private Camera Cam;
    [SerializeField, Range(1f, 10f)]
    public float swayAmount = 2.0f; // How much the camera sways
    public float smoothness = 4.0f;// How smooth the sway is
    public float maxSwayAngle = 10.0f;// Maximum angle of sway

    [SerializeField]
    private First_Person_Data first_Person_Data;


    //Input Variables
    private float Horizontalinput;
    private float Verticalinput;
    private float JumpAxis;

    private Quaternion originalRotation;

    private void Start()
    {
        first_Person_Data = GameObject.FindGameObjectWithTag("GameController").GetComponent<First_Person_Data>();
        Crosshair_adjectment();
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = TopSpeed;
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        Horizontalinput = Input.GetAxis("Horizontal");
        Verticalinput = Input.GetAxis("Vertical");
        JumpAxis = Input.GetAxis("Jump");
        Turn.x += Input.GetAxis("Mouse X") * Sensitivity;
        Turn.y += Input.GetAxis("Mouse Y") * Sensitivity;

        Turn.y = Mathf.Clamp(Turn.y, -50f, 30f);
    }

    private void FixedUpdate()
    {
        Vector3 val = Vector3.up * Turn.x;//This is for left and right camers movement.
        Cam.transform.localRotation = Quaternion.Euler(-Turn.y, val.y, 0);//This is for up and down camera movement
        Ancor.localEulerAngles = val;
        var ro = Cam.transform.localEulerAngles;
        ro.x = Cam.transform.localEulerAngles.x -100;
        Hands.transform.localEulerAngles = ro;
        Hands.transform.localPosition = Vector3.up * 0.32f;

        if (GroundCheck.IsGrounded)
        {
            MovePlayer();
            Animate_camera();
        }
    }

    void MovePlayer()
    {
        if (Horizontalinput != 0)
        {
            rb.AddForce(Speed * Acceleration * Horizontalinput * Cam.transform.right, ForceMode.VelocityChange);
        }
        if (Verticalinput != 0)
        {
            rb.AddForce(Speed * Acceleration * Verticalinput * Ancor.forward, ForceMode.VelocityChange);
        }

        if (Horizontalinput == 0 && Verticalinput == 0)
        {
            if (rb.velocity.magnitude >= DecelerateLimit)
                rb.velocity = new Vector3(rb.velocity.x * Deceleration, rb.velocity.y * Deceleration, rb.velocity.z * Deceleration);
        }
        if (JumpAxis != 0)
        {
            rb.AddForce(Jumpforce * transform.up, ForceMode.Impulse);
        }
    }

    private void Animate_camera()//apply a sway animation
    {
        float targetRotationZ = Horizontalinput * swayAmount;
        targetRotationZ = Mathf.Clamp(targetRotationZ, -maxSwayAngle, maxSwayAngle);
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetRotationZ);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation * targetRotation, smoothness);
    }

    private void Crosshair_adjectment()//Adject the crosshair in the middle of the screen
    {
        var pos = Cam.ViewportToWorldPoint(first_Person_Data.Crosshair_pos_relative_to_viewport);
        pos.z += 1f;
        first_Person_Data.Crosshair.transform.localPosition = Cam.transform.InverseTransformPoint(pos);
    }
};
