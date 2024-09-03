using UnityEngine;
using FirstGearGames.SmoothCameraShaker;
using Unity.VisualScripting;

public class General_Gun_Machanics : MonoBehaviour
{
    [Header("Gun Data Objects"),SerializeField]//These are the Scriptable objects that tell hold all the info about the type of guns
    private Gun_Info Revolver_Info= null;
    [SerializeField]
    private Gun_Info SMG_Info= null;
    [SerializeField]
    private Gun_Info Short_gun_Info= null;
    [SerializeField]
    private Gun_Info Grenade_launcher_Info= null;

    private Gun_Info Current_gun_info = null;

    enum Gun_type {Revolver, SMG, Short_gun, Grenade_launcher, non};
    [SerializeField]
    private Gun_type G_type;//Indecate the type of gun the player is holding

    [Header("ShortGun Related Fields"),SerializeField, Range(4, 10)]
    private int Pellets; // Number of pellets
    [SerializeField, Range(0f, 10f)]
    float Spread_angle = 5f;// Spread angle in degrees

    [Header("Grenade launcher Fields"), SerializeField]
    private GameObject Grenade_prefab;
    [SerializeField]
    private Transform Grenade_spawn_point;
    [SerializeField, Range(6f, 50f)]
    private float Launch_force;

    [Header("Gun Effects"),SerializeField]
    private ShakeData Camera_shake_data;
    [SerializeField]
    private GameObject Muzzel_flash;
    [SerializeField]
    private GameObject impactPrefab;

    private First_Person_Data first_Person_Data;
    private Ui_Handler ui_Handler;
    private Camera cam;
    private float fire_rate_temp;
    private float Scroll_wheel_axis;

    void Start()
    {
        first_Person_Data = GameObject.FindGameObjectWithTag("GameController").GetComponent<First_Person_Data>();
        ui_Handler = GameObject.FindGameObjectWithTag("UIHandler").GetComponent<Ui_Handler>();
        cam = Camera.main;
        Current_gun_info = Revolver_Info;
        fire_rate_temp = Current_gun_info.Fire_rate;
        ui_Handler.Gun_name.text = "Revolver";
    }

    void Update()
    {
        Scroll_wheel_axis = Input.GetAxis("Mouse ScrollWheel");//get the mouse scroll wheel movement

        Change_current_Gun();
        Change_currnet_gun_info();//Changing the gun type on runtime
    }

    void FixedUpdate()
    {
        if (!Current_gun_info || !first_Person_Data)//Safty
            return;
        
        Shoot();
    }

    public void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            var pos = cam.ViewportToWorldPoint(first_Person_Data.Crosshair_pos_relative_to_viewport);
            if (fire_rate_temp <= 0)
            {
                //Applay camera Efects
                fire_rate_temp = Current_gun_info.Fire_rate;
                CameraShakerHandler.Shake(Camera_shake_data);
                Muzzel_flash.SetActive(true);

                if (G_type == Gun_type.Grenade_launcher)//for grenade launcher
                {
                    GameObject grenade = Instantiate(Grenade_prefab, Grenade_spawn_point.position, Grenade_spawn_point.rotation);
        
                    // Get the Rigidbody component to apply force
                    Rigidbody rb = grenade.GetComponent<Rigidbody>();
                    
                    // Apply force to launch the grenade
                    rb.AddForce(cam.transform.forward * Launch_force, ForceMode.VelocityChange);
                }
                else if (G_type == Gun_type.Short_gun)//for short gun
                {
                    for (int i = 0; i < Pellets; i++)
                    {
                        // Calculate random spread direction
                        Vector3 spreadDirection = cam.transform.forward;
                        spreadDirection.x += Random.Range(-Spread_angle, Spread_angle) * 0.01f;
                        spreadDirection.y += Random.Range(-Spread_angle, Spread_angle) * 0.01f;

                        // Fire a ray for each pellet
                        RaycastHit[] result = Physics.RaycastAll(pos, spreadDirection, Current_gun_info.Range, Current_gun_info.Hit_effect_layer);
                        if (result.Length > 0)
                        {
                            // Apply damage and effects for each pellet
                            var hit = result[result.Length - 1];

                            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                            Vector3 hitpos = hit.point;
                            Instantiate(impactPrefab, hitpos, rot);

                            Debug.Log("hit " + hit.collider.name);
                            if (hit.rigidbody)
                                hit.rigidbody.AddForce(spreadDirection * 5f, ForceMode.Impulse);
                        }
                    }
                }
                else//for the rest of the single bullet guns
                {
                    RaycastHit[] result = Physics.RaycastAll(pos, cam.transform.forward, Current_gun_info.Range, Current_gun_info.Hit_effect_layer);
                    if (result.Length > 0)
                    {
                        //Applay Damage
                        var hit = result[result.Length - 1];

                        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                        Vector3 hitpos = hit.point;
                        Instantiate(impactPrefab, hitpos, rot);

                        Debug.Log("hit " + hit.collider.name);
                        if (hit.rigidbody)
                            hit.rigidbody.AddForce(cam.transform.forward * 5f, ForceMode.Impulse);
                    }
                }
            }
        }
        fire_rate_temp -= Time.deltaTime;
    }

    private void Change_currnet_gun_info()
    {
        switch (G_type)
        {
            case Gun_type.Revolver:
                Current_gun_info = Revolver_Info;
                break;
            case Gun_type.SMG:
                Current_gun_info = SMG_Info;
                break;
            case Gun_type.Short_gun:
                Current_gun_info = Short_gun_Info;
                break;
            case Gun_type.Grenade_launcher:
                Current_gun_info = Grenade_launcher_Info;
                break;
            default:
                Current_gun_info = null;
                break;
        }
        Muzzel_flash.GetComponent<ECDisableMe>().Disable_timer = Current_gun_info.Fire_rate;
    }

    public void Change_current_Gun()
    {
        if (Scroll_wheel_axis != 0)
        {
            fire_rate_temp = 0;
            ui_Handler.Text_in(ui_Handler.Gun_name);
        }

        if (Scroll_wheel_axis < 0f && ((int)G_type) < 3)
        {
            G_type++;
            ui_Handler.Gun_name.text = G_type.ToString();
        }
        else if (Scroll_wheel_axis > 0f && ((int)G_type) > 0)
        {
            G_type--;
            ui_Handler.Gun_name.text = G_type.ToString();
        }
        ui_Handler.Text_fade_out(ui_Handler.Gun_name);
    }
}