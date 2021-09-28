using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capcol;

    private Vector3 point1;
    private Vector3 point2;
    private float radius;
    // Start is called before the first frame update
    void Start()
    {
        radius = capcol.radius;
        print(capcol.radius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        point1 = transform.position + transform.up * radius * 0.9f;
        point2 = transform.position + transform.up * capcol.height - transform.up * radius * 0.9f;
        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));
        if (outputCols.Length != 0)
        {
            SendMessageUpwards("IsGround");
            
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point1, (transform.up * radius).magnitude);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point2, (transform.up * radius).magnitude);
    }
}
