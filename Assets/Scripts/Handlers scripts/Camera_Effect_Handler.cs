using UnityEngine;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Camera_effect_Handler : MonoBehaviour
{
    [Header("Camera Shake data")]
    public ShakeData Gun_Recoil_shake;
    public ShakeData Grenade_explosion_shake;
    public ShakeData Dash_shake;

    [Header("Dash Camera Animation")]
    public Animator Dash_anima;
    public Vignette Dash_effect_vignette;

    //Camera variables
    private Camera Main_cam;

    //Handles
    private First_Person_Handler first_Person_Handler;

    void Start()
    {
        Main_cam = Camera.main;
        first_Person_Handler = GameObject.FindGameObjectWithTag("GameController").GetComponent<First_Person_Handler>();
    }

    public void Trigger_Dash_Camera_effect()
    {
        if (first_Person_Handler.Has_dashed)
        {
            Dash_anima.SetTrigger("Play_anime");
            CameraShakerHandler.Shake(Dash_shake);
        }
    }
}