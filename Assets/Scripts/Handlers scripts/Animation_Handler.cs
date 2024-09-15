using UnityEngine;

public class Animation_Handler : MonoBehaviour
{
    public First_Person_Handler first_Person_Handler;

    [Header("Gun animation fields")]
    private float Mousex_input;

    private void Start()
    {
        first_Person_Handler = GameObject.FindGameObjectWithTag("GameController").GetComponent<First_Person_Handler>();
    }

    private void Update()
    {
        Mousex_input = Input.GetAxis("Mouse X");
    }
}
