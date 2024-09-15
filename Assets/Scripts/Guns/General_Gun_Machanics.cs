using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

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

    [Header("SMG Related Fields"), SerializeField, Range(0f, 10f)]
    private float SMG_bullet_spread;

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
    private GameObject Muzzel_flash;
    [SerializeField]
    private GameObject impactPrefab;

    [Header("General Editable Fields"), SerializeField, Range(0.01f, 0.05f)]
    private float X_spread_factor;
    [SerializeField, Range(0.01f, 0.05f)]
    private float Y_spread_factor;
    [SerializeField]
    private GameObject Gun_recoil_partical_effect;

    //Handlers
    private First_Person_Handler first_Person_Data;
    private Ui_Handler ui_Handler;
    private Camera_effect_Handler camera_Effect_Handler;

    private Camera cam;
    private float fire_rate_temp;
    private float Scroll_wheel_axis;
    private Vector3 Guns_originalPosition;
    private Quaternion Guns_originalRotation;


    void Start()
    {
        first_Person_Data = GameObject.FindGameObjectWithTag("GameController").GetComponent<First_Person_Handler>();
        ui_Handler = GameObject.FindGameObjectWithTag("UIHandler").GetComponent<Ui_Handler>();
        camera_Effect_Handler = GameObject.FindGameObjectWithTag("EffectHandler").GetComponent<Camera_effect_Handler>();
        cam = Camera.main;
        Current_gun_info = Revolver_Info;
        fire_rate_temp = Current_gun_info.Fire_rate;
        ui_Handler.Gun_name.text = "Revolver";

        foreach (var item in first_Person_Data.Guns)
        {
            item.SetActive(false);
        }
        first_Person_Data.Current_selected_gun_obj = first_Person_Data.Guns[(int)G_type];
        first_Person_Data.Current_selected_gun_obj.SetActive(true);
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
                CameraShakerHandler.Shake(camera_Effect_Handler.Gun_Recoil_shake);
                Muzzel_flash.SetActive(true);
                first_Person_Data.Current_selected_gun_obj.GetComponent<Animator>().SetTrigger("Play anime");

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
                        spreadDirection.x += Random.Range(-Spread_angle, Spread_angle) * X_spread_factor;
                        spreadDirection.y += Random.Range(-Spread_angle, Spread_angle) * Y_spread_factor;

                        // Fire a ray for each pellet
                        RaycastHit[] result = Physics.RaycastAll(pos, spreadDirection, Current_gun_info.Range, Current_gun_info.Hit_effect_layer);
                        if (result.Length > 0)
                        {
                            // Apply damage and effects for each pellet
                            var hit = result[result.Length - 1];

                            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                            Vector3 hitpos = hit.point;
                            var imp = Instantiate(impactPrefab, hitpos, rot);
                            imp.transform.SetParent(hit.transform);

                            Debug.Log("hit " + hit.collider.name);
                            if (hit.rigidbody)
                                hit.rigidbody.AddForce(spreadDirection * 5f, ForceMode.Impulse);
                        }
                    }
                }
                else//for the rest of the single bullet guns
                {

                    Vector3 spreadDirection = cam.transform.forward;
                    if (G_type == Gun_type.SMG)
                    {
                        spreadDirection.x += Random.Range(-SMG_bullet_spread, SMG_bullet_spread)* X_spread_factor;
                        spreadDirection.y += Random.Range(-SMG_bullet_spread, SMG_bullet_spread)* Y_spread_factor;
                    }

                    RaycastHit[] result = Physics.RaycastAll(pos, spreadDirection, Current_gun_info.Range, Current_gun_info.Hit_effect_layer);
                    if (result.Length > 0)
                    {
                        //Applay Damage
                        var hit = result[result.Length - 1];

                        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                        Vector3 hitpos = hit.point;
                        var imp = Instantiate(impactPrefab, hitpos, rot);
                        imp.transform.SetParent(hit.transform);

                        Debug.Log("hit " + hit.collider.name);
                        if (hit.rigidbody)
                            hit.rigidbody.AddForce(spreadDirection * 5f, ForceMode.Impulse);
                    }
                }
            }
        }
        fire_rate_temp -= Time.deltaTime;
        
        if (Input.GetMouseButtonUp(0))
        {
            Gun_recoil_partical_effect.GetComponent<ParticleSystem>().Play();
        }
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
        if (Scroll_wheel_axis < 0f && ((int)G_type) < 3)
        {
            G_type++;
            ui_Handler.Gun_name.text = G_type.ToString();
            ResetWeaponTransform();
            first_Person_Data.Current_selected_gun_obj.SetActive(false);
            first_Person_Data.Current_selected_gun_obj = first_Person_Data.Guns[(int)G_type];
        }
        else if (Scroll_wheel_axis > 0f && ((int)G_type) > 0)
        {
            G_type--;
            ui_Handler.Gun_name.text = G_type.ToString();
            ResetWeaponTransform();
            first_Person_Data.Current_selected_gun_obj.SetActive(false);
            first_Person_Data.Current_selected_gun_obj = first_Person_Data.Guns[(int)G_type];
        }

        if (Scroll_wheel_axis != 0)
        {
            fire_rate_temp = 0;
            ui_Handler.Text_in(ui_Handler.Gun_name);
            first_Person_Data.Current_selected_gun_obj.SetActive(true);

            // Save new gun's original position and rotation
            SaveWeaponTransform();

            first_Person_Data.Current_selected_gun_obj.GetComponent<Animator>().SetTrigger("Play selected");
        }
        ui_Handler.Text_fade_out(ui_Handler.Gun_name);
    }

    private void SaveWeaponTransform()
    {
        Guns_originalPosition = new Vector3(0,-0.21f,-0.7f);
        Guns_originalRotation = new Quaternion(0,1,0,0);
    }

    private void ResetWeaponTransform()
    {
        first_Person_Data.Current_selected_gun_obj.transform.localPosition = Guns_originalPosition;
        first_Person_Data.Current_selected_gun_obj.transform.localRotation = Guns_originalRotation;
    }
}