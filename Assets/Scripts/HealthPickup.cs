using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthPickup : MonoBehaviour
{
    [SerializeField, Min(1)] private int healAmount = 1;
    [SerializeField] private bool destroyAfterPickup = true;

    private bool wasPickedUp;

    private void Awake()
    {
        EnsureTriggerCollider();
    }

    private void Reset()
    {
        EnsureTriggerCollider();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (wasPickedUp || !other.TryGetComponent(out PlayerHealth playerHealth))
        {
            return;
        }

        wasPickedUp = true;
        playerHealth.RestoreLife(healAmount);

        if (destroyAfterPickup)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void EnsureTriggerCollider()
    {
        if (!TryGetComponent(out Collider2D pickupCollider))
        {
            pickupCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        pickupCollider.isTrigger = true;
    }
}
