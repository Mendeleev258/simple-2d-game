using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameUIBootstrap
{
    private static bool isSubscribed;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (isSubscribed)
        {
            return;
        }

        isSubscribed = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateInitialUI()
    {
        EnsureUI();
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnsureUI();
    }

    private static void EnsureUI()
    {
        PlayerHealthIndicatorUI.EnsureCreated();
        PlayerScoreIndicatorUI.EnsureCreated();
        ControlModeButtonsUI.EnsureCreated();
    }
}
