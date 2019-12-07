using UnityEngine;

public class RayCasting : MonoBehaviour
{

    public Vector3 direction = -Vector3.up;
    public RaycastHit hit;
    public float Maxdistance = 10;
    public LayerMask layermask;
    
    void Start()
    {
        Debug.DrawRay(transform.position, -direction * Maxdistance, Color.white, 10);
        if (Physics.Raycast(transform.position, -direction, out hit, Maxdistance, layermask))
        {
            print(hit.transform.name);
            //commands
        }
    }

    void Update()
    {
        //Debug.DrawRay
        
        
        
        
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}