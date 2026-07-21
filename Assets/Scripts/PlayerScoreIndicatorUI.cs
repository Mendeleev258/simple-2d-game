using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreIndicatorUI : MonoBehaviour
{
    private Text scoreText;
    private PlayerScore playerScore;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void EnsureCreated()
    {
        if (FindFirstObjectByType<PlayerScoreIndicatorUI>() != null)
        {
            return;
        }

        PlayerScore score = FindFirstObjectByType<PlayerScore>();
        if (score == null)
        {
            Debug.LogError("Cannot create score indicator: PlayerScore was not found.");
            return;
        }

        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new("GameUICanvas", typeof(RectTransform));
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);

            canvasObject.AddComponent<GraphicRaycaster>();
        }

        GameObject root = new("PlayerScoreIndicator", typeof(RectTransform), typeof(Image));
        root.transform.SetParent(canvas.transform, false);

        RectTransform rootRect = root.GetComponent<RectTransform>();
        rootRect.anchorMin = new Vector2(0f, 1f);
        rootRect.anchorMax = new Vector2(0f, 1f);
        rootRect.pivot = new Vector2(0f, 1f);
        rootRect.anchoredPosition = new Vector2(24f, -96f);
        rootRect.sizeDelta = new Vector2(210f, 48f);

        Image background = root.GetComponent<Image>();
        background.color = new Color(0.05f, 0.06f, 0.08f, 0.85f);

        GameObject textObject = new("ScoreText", typeof(RectTransform), typeof(Text));
        textObject.transform.SetParent(root.transform, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(14f, 0f);
        textRect.offsetMax = new Vector2(-14f, 0f);

        Text text = textObject.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleLeft;
        text.color = new Color(1f, 0.86f, 0.25f, 1f);

        PlayerScoreIndicatorUI indicator = root.AddComponent<PlayerScoreIndicatorUI>();
        indicator.Initialize(score, text);
    }

    private void OnDestroy()
    {
        if (playerScore != null)
        {
            playerScore.CoinsChanged -= UpdateScore;
        }
    }

    private void Initialize(PlayerScore score, Text text)
    {
        playerScore = score;
        scoreText = text;

        playerScore.CoinsChanged += UpdateScore;
        UpdateScore(playerScore.Coins);
    }

    private void UpdateScore(int coins)
    {
        scoreText.text = $"Coins: {coins}";
    }
}

