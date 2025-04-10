using UnityEngine;
using TMPro;
using System.Collections;
public class ScreenController : MonoBehaviour
{
    [Header("Scrolling Parameters")]
    [Tooltip("Scroll speed in pixels per second.")]
    public float scrollSpeed = 50f;

    // Reference to the Text component and its RectTransform.
    [SerializeField]
    private TextMeshProUGUI uiText;
    private RectTransform textRect;
    // Starting x position and width of the text.
    private float initialX;
    private float textWidth;
    // Width of the parent container (i.e., the screen/panel).
    private float containerWidth;

    void Awake()
    {
        if (uiText == null)
        {
            Debug.LogError("ScrollingText script needs a Text component!");
            enabled = false;
            return;
        }
        textRect = uiText.GetComponent<RectTransform>();
    }

    void Start()
    {
        // Store the initial x-position so we can reset when needed.
        initialX = textRect.anchoredPosition.x;
        // Use a coroutine to wait until the layout is built.
        StartCoroutine(InitializeDimensions());
    }

    IEnumerator InitializeDimensions()
    {
        // Wait one frame for layout to update.
        yield return null;
        // Get the width of the text (the entire content).
        textWidth = textRect.rect.width;

        // Assume the parent of the text object is the container.
        RectTransform parentRect = textRect.parent.GetComponent<RectTransform>();
        if (parentRect != null)
        {
            containerWidth = parentRect.rect.width;
        }
        else
        {
            // Fallback to screen width if parent is missing.
            containerWidth = Screen.width;
        }
    }

    void Update()
    {
        if (textRect == null) return;

        // Move the text leftwards based on scrollSpeed and frame time.
        float newX = textRect.anchoredPosition.x - scrollSpeed * Time.deltaTime;
        textRect.anchoredPosition = new Vector2(newX, textRect.anchoredPosition.y);

        // Check if the text has completely scrolled off the left side.
        if (newX <= -textWidth)
        {
            // Reset the position to just beyond the right edge of the container.
            textRect.anchoredPosition = new Vector2(containerWidth, textRect.anchoredPosition.y);
        }
    }
}
