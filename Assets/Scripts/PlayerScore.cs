using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField, Min(0)] private int initialCoins;

    public int Coins { get; private set; }

    public event Action<int> CoinsChanged;

    private void Awake()
    {
        Coins = initialCoins;
        CoinsChanged?.Invoke(Coins);
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Coins += amount;
        CoinsChanged?.Invoke(Coins);
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || Coins < amount)
        {
            return false;
        }

        Coins -= amount;
        CoinsChanged?.Invoke(Coins);
        return true;
    }

    public void ResetCoins()
    {
        Coins = initialCoins;
        CoinsChanged?.Invoke(Coins);
    }
}
