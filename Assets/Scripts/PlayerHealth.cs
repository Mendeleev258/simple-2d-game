using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxLives = 3;
    [SerializeField] private bool reloadSceneOnDeath = true;

    public int MaxLives => maxLives;
    public int CurrentLives { get; private set; }
    public bool IsDead => CurrentLives <= 0;

    public event Action<int, int> LivesChanged;
    public event Action Died;

    private void Awake()
    {
        CurrentLives = maxLives;
        LivesChanged?.Invoke(CurrentLives, maxLives);
    }

    public void TakeDamage(int amount = 1)
    {
        if (amount <= 0 || IsDead)
        {
            return;
        }

        CurrentLives = Mathf.Max(CurrentLives - amount, 0);
        LivesChanged?.Invoke(CurrentLives, maxLives);

        if (IsDead)
        {
            Die();
        }
    }

    public void RestoreLife(int amount = 1)
    {
        if (amount <= 0 || IsDead)
        {
            return;
        }

        CurrentLives = Mathf.Min(CurrentLives + amount, maxLives);
        LivesChanged?.Invoke(CurrentLives, maxLives);
    }

    public void ResetLives()
    {
        CurrentLives = maxLives;
        LivesChanged?.Invoke(CurrentLives, maxLives);
    }

    private void Die()
    {
        Died?.Invoke();

        if (reloadSceneOnDeath)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}
