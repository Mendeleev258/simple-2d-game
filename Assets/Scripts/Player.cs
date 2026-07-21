using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputReader))]
public class Player : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 10f;

    private Rigidbody2D rb;
    private PlayerInputReader inputReader;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputReader = GetComponent<PlayerInputReader>();

        if (inputReader == null)
        {
            inputReader = gameObject.AddComponent<PlayerInputReader>();
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = Vector2.ClampMagnitude(inputReader.Movement, 1f);
        Vector2 movement = direction * movingSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
    }
}
