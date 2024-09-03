using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    float timer;
    public float Destroy_timer = 10;
	
	// Update is called once per frame
	void Update ()
   {
       timer += Time.deltaTime;

       if(timer >= Destroy_timer)
       {
            timer = 0;
            Destroy(gameObject);
       }
	}
}
