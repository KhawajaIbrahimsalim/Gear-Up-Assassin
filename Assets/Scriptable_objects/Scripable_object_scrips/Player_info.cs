using UnityEngine;

[CreateAssetMenu(fileName = "Player data", menuName = "Player Data", order = 51)]
public class Player_info : ScriptableObject
{
    public float health;
    public float speed;
    public float Steam;
    [Range(0, 7)]
    public int Steam_cans;
    [Range(0, 10)]
    public int Healh_packs;
    [Range(0.1f, 5f)]
    public float Dash_power;
}
