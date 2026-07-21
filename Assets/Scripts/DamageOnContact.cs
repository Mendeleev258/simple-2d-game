using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageOnContact : MonoBehaviour
{
    [SerializeField, Min(1)] private int damage = 1;
    [SerializeField] private bool destroyAfterHit;

    private void Awake()
    {
        EnsureTriggerCollider();
    }

    private void Reset()
    {
        EnsureTriggerCollider();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamage(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDamage(other.gameObject);
    }

    private void TryDamage(GameObject target)
    {
        if (!target.TryGetComponent(out PlayerHealth playerHealth))
        {
            return;
        }

        playerHealth.TakeDamage(damage);

        if (destroyAfterHit)
        {
            Destroy(gameObject);
        }
    }

    private void EnsureTriggerCollider()
    {
        if (!TryGetComponent(out Collider2D contactCollider))
        {
            contactCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        contactCollider.isTrigger = true;
    }
}
