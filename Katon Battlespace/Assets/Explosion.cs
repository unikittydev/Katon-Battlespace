using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private SphereCollider trigger;

    [HideInInspector]
    public float damage;

    private void Start()
    {
        trigger.radius = damage / VoxelDamage.damageFactor;
        Destroy(gameObject, 1f);
    }
}
