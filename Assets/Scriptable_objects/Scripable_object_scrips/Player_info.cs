using UnityEngine;

[CreateAssetMenu(fileName = "Player data", menuName = "Player Data", order = 51)]
public class Player_info : ScriptableObject
{
    [Range(50, 300)]
    public int Health;
    [Range(0.5f, 5f)]
    public float Speed;
    public float TopSpeed;
    [Range(0.1f, 1f)]
    public float Acceleration;
    [Range(0.01f, 1f)]
    public float Deceleration;
    public float Jumpforce;
    [Range(0f, 50f)]
    public float Dash_force = 5f;
    [Range(5, 50)]
    public float Steam;
    [Range(0, 7)]
    public int Steam_cans;
    [Range(0, 10)]
    public int Healh_packs;
}
