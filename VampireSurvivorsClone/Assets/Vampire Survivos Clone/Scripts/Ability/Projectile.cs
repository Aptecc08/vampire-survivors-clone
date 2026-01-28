using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 velocity;
    [SerializeField] private float _damage;
    public void Initialize(Vector2 direction, float speed, float damage)
    {
        velocity = direction * speed;
        _damage = damage;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ApplyDamage(other);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ApplyDamage(collision.collider);
    }

    private void ApplyDamage(Collider2D other)
    {
        if (other == null || !other.CompareTag("Enemy"))
            return;

        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.TakeDamage(_damage);
    }

    private void Update()
    {
        transform.position += (Vector3)(velocity * Time.deltaTime);
    }
}
