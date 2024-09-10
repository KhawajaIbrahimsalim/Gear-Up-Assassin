using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class Grenade : MonoBehaviour
{
    [Header("Grenade Fields"), SerializeField, Range(1f, 50f)]
    private float Explosion_radius;
    [SerializeField]
    private GameObject Explosion_effect;
    [SerializeField, Range(0.1f, 5f)]
    private float Delay;
    [SerializeField, Range(10f, 100f)]
    private float Explosion_force;
    [SerializeField, Range(1f, 10f)]
    private float Explosion_upward_force = 4;
    [SerializeField]
    private LayerMask Effected_layer;

    private Camera_effect_Handler camera_Effect_Handler;

    private float Countdown;
    private bool Hasexploded = false;

    void Start()
    {
        camera_Effect_Handler = GameObject.FindGameObjectWithTag("EffectHandler").GetComponent<Camera_effect_Handler>();
        Countdown = Delay;
    }

    void Update()
    {
        Countdown -= Time.deltaTime;
        if (Countdown <= 0f && !Hasexploded)
        {
            Explode_grenade();
            Hasexploded = true;
        }
    }

    public void Explode_grenade()
    {
        Instantiate(Explosion_effect, transform.position, Quaternion.identity);
        CameraShakerHandler.Shake(camera_Effect_Handler.Grenade_explosion_shake);

        var colliders = Physics.OverlapSphere(transform.position, Explosion_radius, Effected_layer);
        
        foreach (var hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("launch " + hit.name);
                rb.AddExplosionForce(Explosion_force, transform.position, Explosion_radius, Explosion_upward_force, ForceMode.VelocityChange);
            }

            //apply damage to nearby enemyes
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Explosion_radius);
    }
}
