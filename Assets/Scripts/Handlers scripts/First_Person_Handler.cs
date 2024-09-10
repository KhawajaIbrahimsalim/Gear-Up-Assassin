using UnityEngine;

public class First_Person_Handler : MonoBehaviour
{
    [Header("Crosshair Fields")]
    public Vector3 Crosshair_pos_relative_to_viewport = new Vector3(0.5f, 0.5f, 0);
    public Transform Crosshair;

    [Header("Movement Fields"), HideInInspector]
    public bool Has_dashed = false;

    void Start()
    {
    }
}
