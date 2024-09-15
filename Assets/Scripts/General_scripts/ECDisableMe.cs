using UnityEngine;

public class ECDisableMe : MonoBehaviour{

   float timer;
   public float Disable_timer = 10;
	
	// Update is called once per frame
	void Update ()
   {
       timer += Time.deltaTime;

       if(timer >= Disable_timer)
       {
            timer = 0;
            gameObject.SetActive(false);
       }
	
	}
}
