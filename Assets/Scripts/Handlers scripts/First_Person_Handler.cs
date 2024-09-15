using System.Collections.Generic;
using UnityEngine;

public class First_Person_Handler : MonoBehaviour
{
    [Header("Crosshair Fields")]
    public Vector3 Crosshair_pos_relative_to_viewport = new Vector3(0.5f, 0.5f, 0);
    public Transform Crosshair;

    [Header("Movement Fields"), HideInInspector]
    public bool Has_dashed = false;

    [Header("Data Members")]
    public List<GameObject> Guns;
    [HideInInspector]
    public GameObject Current_selected_gun_obj;

    void Start()
    {
    }
}
