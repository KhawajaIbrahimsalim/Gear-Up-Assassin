using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class Grenade : MonoBehaviour
{
    [Header("Grenade Fields"), SerializeField, Range(1f, 50f)]
    private float Explosion_radius;
    [SerializeField]
    private GameObject Explosion_effect;
    [SerializeField]
    private ShakeData Camera_shake_data;
    [SerializeField, Range(0.1f, 5f)]
    private float Delay;
    [SerializeField, Range(10f, 100f)]
    private float Explosion_force;
    [SerializeField]
    private LayerMask Effected_layer;

    private float Countdown;
    private bool Hasexploded = false;

    void Start()
    {
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
        CameraShakerHandler.Shake(Camera_shake_data);

        var colliders = Physics.OverlapSphere(transform.position, Explosion_radius, Effected_layer);
        
        foreach (var hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("launch " + hit.name);
                rb.AddExplosionForce(Explosion_force, transform.position, Explosion_radius, 5, ForceMode.VelocityChange);
            }

            //apply damage to nearby enemyes
        }
    }
}
