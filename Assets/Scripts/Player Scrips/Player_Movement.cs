using FirstGearGames.SmoothCameraShaker;
using UnityEngine;

class Player_Movement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField]
    private Player_info player_Info;
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
    [Range(1f, 10f)]
    public float swayAmount = 2.0f; // How much the camera sways
    public float smoothness = 4.0f;// How smooth the sway is
    public float maxSwayAngle = 10.0f;// Maximum angle of sway
    [SerializeField, Range(-20f, 20f)]
    private float adject_Hands_angle = 0f;
    [SerializeField, Range(-2f, 2f)]
    private float adject_Hands_pos = 0f;

    //Handlers
    private First_Person_Handler first_Person_Data;
    private Camera_effect_Handler camera_Effect_Handler;


    //Input Variables
    private float Horizontalinput;
    private float Verticalinput;
    private float JumpAxis;

    private Quaternion originalRotation;

    private void Start()
    {
        first_Person_Data = GameObject.FindGameObjectWithTag("GameController").GetComponent<First_Person_Handler>();
        camera_Effect_Handler = GameObject.FindGameObjectWithTag("EffectHandler").GetComponent<Camera_effect_Handler>();
        Crosshair_adjectment();
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        SetMouse_axis();
    }

    private void FixedUpdate()
    {
        Move_camera_and_ancors();

        if (GroundCheck.IsGrounded)
        {
            MovePlayer();
            Dash_move();
            Animate_camera();
        }
        //Debug.Log(rb.velocity.magnitude);
    }

    void SetMouse_axis()
    {
        Horizontalinput = Input.GetAxis("Horizontal");
        Verticalinput = Input.GetAxis("Vertical");
        JumpAxis = Input.GetAxis("Jump");
        Turn.x += Input.GetAxis("Mouse X") * Sensitivity;
        Turn.y += Input.GetAxis("Mouse Y") * Sensitivity;

        Turn.y = Mathf.Clamp(Turn.y, -50f, 30f);
    }

    void Move_camera_and_ancors()
    {
        Vector3 val = Vector3.up * Turn.x;//This is for left and right camers movement.
        Cam.transform.localRotation = Quaternion.Euler(-Turn.y, val.y, 0);//This is for up and down camera movement

        Ancor.localEulerAngles = val;

        var ro = Cam.transform.localEulerAngles;
        ro.x = Cam.transform.localEulerAngles.x + adject_Hands_angle;//help adject the angle of the gun in runtime
        Hands.transform.localEulerAngles = ro;
        Hands.transform.localPosition = Vector3.up * adject_Hands_pos;//help adject the position of the gun in runtime
    }

    void MovePlayer()
    {
        if (Horizontalinput != 0)
        {
            rb.AddForce(player_Info.Speed * player_Info.Acceleration * Horizontalinput * Cam.transform.right, ForceMode.VelocityChange);
        }
        if (Verticalinput != 0)
        {
            rb.AddForce(player_Info.Speed * player_Info.Acceleration * Verticalinput * Ancor.forward, ForceMode.VelocityChange);
        }

        if ((Horizontalinput == 0 && Verticalinput == 0) || rb.velocity.magnitude >= player_Info.TopSpeed)
        {
            var slowdown = player_Info.Deceleration;

            if (rb.velocity.magnitude >= player_Info.TopSpeed)
                slowdown = 0.75f;

            rb.velocity *= slowdown;
        }
        if (JumpAxis != 0)
        {
            rb.AddForce(player_Info.Jumpforce * transform.up, ForceMode.Impulse);
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

    void Dash_move()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            first_Person_Data.Has_dashed = true;
            camera_Effect_Handler.Trigger_Dash_Camera_effect();
            var move_direction = Vector3.zero;

            if (Horizontalinput != 0)
            {
                move_direction += Cam.transform.right * Horizontalinput;
            }
            if (Verticalinput != 0)
            {
                move_direction += Ancor.forward * Verticalinput;
            }

            if (move_direction != Vector3.zero)
            {
                move_direction.Normalize();
            }
            else
            {
                // If no input, dash forward based on camera direction
                move_direction = Cam.transform.forward;
            }

            move_direction *= player_Info.Dash_force;
            //Debug.Log(move_direction);
            first_Person_Data.Has_dashed = false;
            rb.AddForce(move_direction, ForceMode.Impulse); //Applys the force here
        }
    }
};
