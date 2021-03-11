using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rb;
    public Rigidbody rb => _rb;

    [SerializeField]
    private Explosion explosionPrefab;

    public float startForce;
    public float damage;

    private void OnCollisionEnter()
    {
        Explosion explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.damage = damage;
        Destroy(gameObject);
    }
}
