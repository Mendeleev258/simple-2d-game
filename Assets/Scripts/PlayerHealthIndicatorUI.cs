using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthIndicatorUI : MonoBehaviour
{
    private Image fillImage;
    private Text healthText;
    private PlayerHealth playerHealth;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void EnsureCreated()
    {
        if (FindFirstObjectByType<PlayerHealthIndicatorUI>() != null)
        {
            return;
        }

        PlayerHealth health = FindFirstObjectByType<PlayerHealth>();
        if (health == null)
        {
            Debug.LogError("Cannot create health indicator: PlayerHealth was not found.");
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

        GameObject root = new("PlayerHealthIndicator", typeof(RectTransform), typeof(Image));
        root.transform.SetParent(canvas.transform, false);

        RectTransform rootRect = root.GetComponent<RectTransform>();
        rootRect.anchorMin = new Vector2(0f, 1f);
        rootRect.anchorMax = new Vector2(0f, 1f);
        rootRect.pivot = new Vector2(0f, 1f);
        rootRect.anchoredPosition = new Vector2(24f, -24f);
        rootRect.sizeDelta = new Vector2(300f, 64f);

        Image background = root.GetComponent<Image>();
        background.color = new Color(0.05f, 0.06f, 0.08f, 0.85f);

        GameObject barBackgroundObject = new("BarBackground", typeof(RectTransform), typeof(Image));
        barBackgroundObject.transform.SetParent(root.transform, false);

        RectTransform barBackgroundRect = barBackgroundObject.GetComponent<RectTransform>();
        barBackgroundRect.anchorMin = new Vector2(0f, 0.5f);
        barBackgroundRect.anchorMax = new Vector2(1f, 0.5f);
        barBackgroundRect.pivot = new Vector2(0.5f, 0.5f);
        barBackgroundRect.anchoredPosition = new Vector2(0f, -8f);
        barBackgroundRect.sizeDelta = new Vector2(-28f, 22f);

        Image barBackground = barBackgroundObject.GetComponent<Image>();
        barBackground.color = new Color(0.18f, 0.2f, 0.25f, 1f);

        GameObject fillObject = new("Fill", typeof(RectTransform), typeof(Image));
        fillObject.transform.SetParent(barBackgroundObject.transform, false);

        RectTransform fillRect = fillObject.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        Image fill = fillObject.GetComponent<Image>();
        fill.color = new Color(0.16f, 0.75f, 0.35f, 1f);
        fill.type = Image.Type.Filled;
        fill.fillMethod = Image.FillMethod.Horizontal;
        fill.fillOrigin = (int)Image.OriginHorizontal.Left;

        GameObject textObject = new("HealthText", typeof(RectTransform), typeof(Text));
        textObject.transform.SetParent(root.transform, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 1f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.pivot = new Vector2(0.5f, 1f);
        textRect.anchoredPosition = new Vector2(0f, -5f);
        textRect.sizeDelta = new Vector2(-28f, 28f);

        Text text = textObject.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 20;
        text.alignment = TextAnchor.MiddleLeft;
        text.color = Color.white;

        PlayerHealthIndicatorUI indicator = root.AddComponent<PlayerHealthIndicatorUI>();
        indicator.Initialize(health, fill, text);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.LivesChanged -= UpdateIndicator;
        }
    }

    private void Initialize(PlayerHealth health, Image fill, Text text)
    {
        playerHealth = health;
        fillImage = fill;
        healthText = text;

        playerHealth.LivesChanged += UpdateIndicator;
        UpdateIndicator(playerHealth.CurrentLives, playerHealth.MaxLives);
    }

    private void UpdateIndicator(int currentLives, int maxLives)
    {
        float fillAmount = maxLives <= 0 ? 0f : (float)currentLives / maxLives;
        fillImage.fillAmount = fillAmount;
        healthText.text = $"HP {currentLives}/{maxLives}";

        fillImage.color = fillAmount > 0.5f
            ? new Color(0.16f, 0.75f, 0.35f, 1f)
            : new Color(0.9f, 0.28f, 0.18f, 1f);
    }
}

