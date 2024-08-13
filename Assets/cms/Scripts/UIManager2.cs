using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager2 : MonoBehaviour
{
    public RectTransform uiGroup; // Parent containing the UI panel and button
    public Button toggleButton; // Toggle button

    private bool isPanelVisible = false; // Current state of the panel

    private Vector2 hiddenPosition; // Position when the group is hidden
    private Vector2 visiblePosition; // Position when the group is visible

    private float animationDuration = 0.5f; // Duration of the animation

    void Start()
    {
        // Connect the button click event to the method
        toggleButton.onClick.AddListener(TogglePanelVisibility);

        // Set initial positions
        hiddenPosition = new Vector2(uiGroup.anchoredPosition.x, -uiGroup.rect.height);
        visiblePosition = new Vector2(uiGroup.anchoredPosition.x, 0);

        // Initially place the UI group at the hidden position
        uiGroup.anchoredPosition = hiddenPosition;
    }

    // Method called on button click
    private void TogglePanelVisibility()
    {
        isPanelVisible = !isPanelVisible;

        // Handle animation
        StopAllCoroutines(); // Stop any currently running coroutines
        StartCoroutine(AnimatePanel(isPanelVisible ? visiblePosition : hiddenPosition));
    }

    private IEnumerator AnimatePanel(Vector2 targetPosition)
    {
        Vector2 startPosition = uiGroup.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            uiGroup.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / animationDuration);
            yield return null;
        }

        uiGroup.anchoredPosition = targetPosition;
    }
}