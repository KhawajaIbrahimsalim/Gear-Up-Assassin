using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player; // the player object
    public GameObject Model;
    public float Sensativity = 10.0f;
    public float ModelView = 15f;

    private Vector3 MouseAxis = new Vector3();

    void Start()
    {

    }

    void Update()
    {
        CameraFollow();
    }

    void CameraFollow()
    {
        #region Align_Camera_Axis_with_Player_Axis

        // Camera move with mouse
        MouseAxis.x += Input.GetAxis("Mouse X") * Sensativity;
        MouseAxis.y += Input.GetAxis("Mouse Y") * Sensativity;

        MouseAxis.y = Mathf.Clamp(MouseAxis.y, -50, 30);
        Camera.main.transform.localRotation = Quaternion.Euler(-MouseAxis.y, 0f, 0f); // Camera move along vertical
        // Alternative to making the Model child of the Camera for Animating the Camera without effecting the child.
        Model.transform.localEulerAngles = 
            new Vector3(transform.localEulerAngles.x - ModelView, 
            transform.localEulerAngles.y, 
            transform.localEulerAngles.z);

        float X_ontheaxisof_Y = (MouseAxis.x);
        //float Y_ontheaxisof_X = (-MouseAxis.y);

        Player.transform.localEulerAngles = new Vector3(0f, X_ontheaxisof_Y, 0); // Camera move along Horizontal

        #endregion Align_Camera_Axis_with_Player_Axis
    }
}

#region Important Information
// Alternative to making the Camera child of the Player.
//Camera.main.transform.localEulerAngles = Player.transform.localEulerAngles;

// Trying to changing the local rotation of the Player.
//Player.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
#endregion Important Information

// NOTE: As Player is the Root it has colliders and rigid body so it is only rotating the horizontal direction
// and Camera is moving the model in vertical direction. It is possible that player is the one to effect the
// vertical and horizontal direction but the PROBLEM is the colliders move along as the player rotate/look in
// the verical direction.