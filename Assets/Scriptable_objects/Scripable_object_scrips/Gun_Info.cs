using UnityEngine;

[CreateAssetMenu(fileName = "Gun data", menuName = "Gun_info", order = 50)]
public class Gun_Info : ScriptableObject
{
    [Range(10f, 100f)]
    public float Damage;
    [Range(0.01f, 5f)]
    public float Fire_rate;
    [Range(5, 100), Tooltip("Sets the length of the raycast (This field does not effect grenade launcher)")]
    public float Range;
    public LayerMask Hit_effect_layer;
    public LayerMask Damage_effect_layer;
    [Range(2, 100)]
    public int Clip_size;
    [Range(0.1f, 5f)]
    public float Reload_time;
}
