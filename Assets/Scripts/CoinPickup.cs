using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CoinPickup : MonoBehaviour
{
    [SerializeField, Min(1)] private int value = 1;
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
        if (wasPickedUp || !other.TryGetComponent(out PlayerScore playerScore))
        {
            return;
        }

        wasPickedUp = true;
        playerScore.AddCoins(value);

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
        BoxCollider2D coinCollider = GetComponent<BoxCollider2D>();

        if (coinCollider == null)
        {
            coinCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        coinCollider.isTrigger = true;
    }
}
