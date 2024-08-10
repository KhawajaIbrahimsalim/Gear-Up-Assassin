using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    [Header("Fire Damage")]
    public float Damage = 100f;

    //public float Speed;

    //private Rigidbody rb;
    //private GameObject ProjectileSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        //ProjectileSpawnPoint = GameObject.Find("ProjectileSpawnPoint");

        //Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.AddForce(ProjectileSpawnPoint.transform.forward * Speed * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HomeLess"))
        {
            other.gameObject.GetComponent<EnemyProperties>().MaxHealth -= Damage;

            if (other.gameObject.GetComponent<EnemyProperties>().MaxHealth <= 0f)
                Destroy(other.gameObject);
        }
    }
}
