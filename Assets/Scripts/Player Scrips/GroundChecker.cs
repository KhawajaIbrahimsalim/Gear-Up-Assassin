using UnityEngine;

class GroundChecker : MonoBehaviour
{
    public bool IsGrounded = false;
    public LayerMask solidlayer; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Collider[] objects =  Physics.OverlapBox(transform.position, new Vector3(0.4f, 0.06f, 0.4f) / 2, transform.rotation, solidlayer);

        if (objects.Length <= 0)
        {
            IsGrounded = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(0.4f, 0.06f, 0.4f));
    }
};