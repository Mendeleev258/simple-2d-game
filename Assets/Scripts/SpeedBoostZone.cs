using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SpeedBoostZone : MonoBehaviour
{
    [SerializeField, Min(1f)] private float speedMultiplier = 1.5f;
    [SerializeField, Min(0.1f)] private float duration = 3f;

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
        if (!other.TryGetComponent(out Player player))
        {
            return;
        }

        player.ApplySpeedMultiplier(speedMultiplier, duration);
    }

    private void EnsureTriggerCollider()
    {
        BoxCollider2D boostCollider = GetComponent<BoxCollider2D>();

        if (boostCollider == null)
        {
            boostCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        boostCollider.isTrigger = true;
    }
}
