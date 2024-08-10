using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject Projectile;
    public GameObject ProjectileSpawnPoint;
    public float Speed;

    private GameObject _Projectile;
    // Start is called before the first frame update
    void Start()
    {
        _Projectile = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && _Projectile == null)
        {
            _Projectile = Instantiate(Projectile,
            ProjectileSpawnPoint.transform.position,
            ProjectileSpawnPoint.transform.rotation);

            _Projectile.transform.SetParent(ProjectileSpawnPoint.transform);

            Destroy(_Projectile, 3f);
        }
    }
}
