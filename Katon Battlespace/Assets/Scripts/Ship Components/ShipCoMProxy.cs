using UnityEngine;

public class ShipCoMProxy : MonoBehaviour
{
    private Rigidbody parentRb;
    private Transform tr;

    private void Awake()
    {
        tr = transform;
        parentRb = tr.parent.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        tr.localPosition = parentRb.centerOfMass;
    }
}
