using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


//Detects interactable objects in front of the player using raycasting
//Requires colliders on objects and InteractableInterface for interactions
public class Interactor : MonoBehaviour
{   
    //Max distance in Unity units the player can interact with objects
    public float interactDistance = 2f;

    //References the currently detected interactable object and clock
    private InteractableInterface current;
    private ClockInteractable currentClock;
    private float hover;

    private Image ring;
    private CanvasGroup prompt;
    private Text label;
    private RectTransform key;

    //Creates the interaction prompt and crosshair UI when the game starts
    void Start()
    {
        BuildHud();
    }

    
    void Update()
    {
        current = null;
        currentClock = null;

        //Casts a ray forward to find an object within interaction distance
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactDistance))
        {
            current = hit.collider.GetComponentInParent<InteractableInterface>(); //Selects the object's InteractableInterface if interaction is allowed
            currentClock = hit.collider.GetComponentInParent<ClockInteractable>();
        }

        if (current != null && !current.CanInteract)
            current = null;

        //show/hide the clock UI
        if (HUDManager.Instance != null)
        {
            if (currentClock != null && !SleepFader.Busy)
                HUDManager.Instance.ShowClock();
            else
                HUDManager.Instance.HideClock();
        }

        //Pressing E interacts with the selected object
        if (current != null && !SleepFader.Busy && Keyboard.current.eKey.wasPressedThisFrame) {
            if (ThoughtUI.Instance != null && ThoughtUI.Instance.IsShowing)
                return;

            current.Interact();
        }

        if (current != null)
        {
            label.text = current.Prompt;
            key.anchoredPosition = new Vector2(-label.preferredWidth / 2f - 6f, 0f);
        }

        //Disables interactions during sleep transitions of active thoughts
        bool show = current != null && !SleepFader.Busy;
        hover = Mathf.MoveTowards(hover, show ? 1f : 0f, 8f * Time.deltaTime);
        ring.color = new Color(1f, 1f, 1f, hover);
        prompt.alpha = hover;
    }

    //Creates the interaction HUD, including the crosshair and E-key prompt
    void BuildHud()
    {
        var canvas = new GameObject("InteractionHUD").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        CreateImage("Dot", canvas.transform, 6f).sprite = CircleSprite(0f);
        ring = CreateImage("Ring", canvas.transform, 26f);
        ring.sprite = CircleSprite(5f);

        prompt = new GameObject("Prompt", typeof(RectTransform)).AddComponent<CanvasGroup>();
        prompt.transform.SetParent(canvas.transform, false);
        ((RectTransform)prompt.transform).anchoredPosition = new Vector2(0f, -60f);

        label = CreateText("Label", prompt.transform, 24);
        label.rectTransform.anchoredPosition = new Vector2(24f, 0f);

        var keyBox = CreateImage("Key", prompt.transform, 36f);
        keyBox.color = new Color(0f, 0f, 0f, 0.55f);
        keyBox.gameObject.AddComponent<Outline>().effectColor = Color.white;
        key = keyBox.rectTransform;
        CreateText("E", key, 20).text = "E";
    }

    Image CreateImage(string name, Transform parent, float size)
    {
        var image = new GameObject(name).AddComponent<Image>();
        image.transform.SetParent(parent, false);
        image.rectTransform.sizeDelta = new Vector2(size, size);
        return image;
    }

    Text CreateText(string name, Transform parent, int fontSize)
    {
        var text = new GameObject(name).AddComponent<Text>();
        text.transform.SetParent(parent, false);
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.gameObject.AddComponent<Shadow>();
        return text;
    }

    static Sprite CircleSprite(float ringWidth)
    {
        const int size = 64;
        var texture = new Texture2D(size, size);
        float c = (size - 1) / 2f;
        float r = size / 2f - 1f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float d = Vector2.Distance(new Vector2(x, y), new Vector2(c, c));
                float a = Mathf.Clamp01(r - d + 0.5f);
                if (ringWidth > 0f)
                    a *= Mathf.Clamp01(d - (r - ringWidth) + 0.5f);
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, a));
            }
        }
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }
}