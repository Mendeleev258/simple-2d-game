using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlModeButtonsUI : MonoBehaviour
{
    private static readonly Color NormalColor = new(0.18f, 0.20f, 0.25f, 0.9f);
    private static readonly Color SelectedColor = new(0.16f, 0.55f, 0.32f, 1f);

    private readonly Dictionary<PlayerControlMode, Image> buttonImages = new();
    private PlayerInputReader inputReader;
    private GameObject joystickRoot;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateMenu()
    {
        if (FindFirstObjectByType<ControlModeButtonsUI>() != null)
        {
            return;
        }

        PlayerInputReader reader = FindFirstObjectByType<PlayerInputReader>();
        if (reader == null)
        {
            Debug.LogError("Невозможно создать меню управления: PlayerInputReader не найден.");
            return;
        }

        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new("ControlsCanvas", typeof(RectTransform));
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        GameObject menuObject = new("ControlModeMenu", typeof(RectTransform), typeof(Image));
        menuObject.transform.SetParent(canvas.transform, false);

        RectTransform menuRect = menuObject.GetComponent<RectTransform>();
        menuRect.anchorMin = new Vector2(1f, 1f);
        menuRect.anchorMax = new Vector2(1f, 1f);
        menuRect.pivot = new Vector2(1f, 1f);
        menuRect.anchoredPosition = new Vector2(-24f, -24f);
        menuRect.sizeDelta = new Vector2(280f, 210f);

        menuObject.GetComponent<Image>().color = new Color(0.05f, 0.06f, 0.08f, 0.85f);

        ControlModeButtonsUI menu = menuObject.AddComponent<ControlModeButtonsUI>();
        menu.Build(reader);
    }

    private void Build(PlayerInputReader reader)
    {
        inputReader = reader;
        joystickRoot = GameObject.Find("JoystickBackground");

        CreateLabel("Управление", new Vector2(0f, -12f), 26);
        CreateButton("Клавиатура", PlayerControlMode.Keyboard, -58f);
        CreateButton("Джойстик", PlayerControlMode.Joystick, -108f);
        CreateButton("Гироскоп", PlayerControlMode.Gyroscope, -158f);

        ApplyMode(inputReader.ControlMode);
    }

    private void CreateLabel(string caption, Vector2 position, int fontSize)
    {
        GameObject labelObject = new("Title", typeof(RectTransform), typeof(Text));
        labelObject.transform.SetParent(transform, false);

        RectTransform rect = labelObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(-20f, 38f);

        Text text = labelObject.GetComponent<Text>();
        text.text = caption;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
    }

    private void CreateButton(string caption, PlayerControlMode mode, float positionY)
    {
        GameObject buttonObject = new(caption, typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(transform, false);

        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, positionY);
        rect.sizeDelta = new Vector2(-24f, 42f);

        Image image = buttonObject.GetComponent<Image>();
        image.color = NormalColor;
        buttonImages.Add(mode, image);

        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = image;
        button.onClick.AddListener(() => ApplyMode(mode));

        GameObject textObject = new("Text", typeof(RectTransform), typeof(Text));
        textObject.transform.SetParent(buttonObject.transform, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text text = textObject.GetComponent<Text>();
        text.text = caption;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 21;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
    }

    private void ApplyMode(PlayerControlMode mode)
    {
        inputReader.SetControlMode(mode);

        foreach (KeyValuePair<PlayerControlMode, Image> entry in buttonImages)
        {
            entry.Value.color = entry.Key == mode ? SelectedColor : NormalColor;
        }

        if (joystickRoot != null)
        {
            joystickRoot.SetActive(mode == PlayerControlMode.Joystick);
        }
    }
}
