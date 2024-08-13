using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Tilemaps;

public class UIManager2 : MonoBehaviour
{
    public RectTransform uiGroup; // Parent containing the UI panel and button
    public Button toggleButton; // Toggle button
    public Button stationButton; // Station button
    public GameObject stationPrefab; // Station prefab to spawn
    public Tilemap roadTilemap; // Reference to the Tilemap where the prefab should be placed

    private bool isPanelVisible = false; // Current state of the panel
    private Vector2 hiddenPosition; // Position when the group is hidden
    private Vector2 visiblePosition; // Position when the group is visible
    private float animationDuration = 0.5f; // Duration of the animation
    private CanvasGroup uiGroupCanvasGroup; // CanvasGroup for the UI panel

    private void Start()
    {
        // Initialize CanvasGroup for UI group
        uiGroupCanvasGroup = uiGroup.GetComponent<CanvasGroup>();
        if (uiGroupCanvasGroup == null)
        {
            uiGroupCanvasGroup = uiGroup.gameObject.AddComponent<CanvasGroup>();
        }

        // Connect the button click event to the method
        toggleButton.onClick.AddListener(TogglePanelVisibility);

        // Set initial positions
        hiddenPosition = new Vector2(uiGroup.anchoredPosition.x, -uiGroup.rect.height);
        visiblePosition = new Vector2(uiGroup.anchoredPosition.x, 0);

        // Initially place the UI group at the hidden position
        uiGroup.anchoredPosition = hiddenPosition;

        // Add drag events to the station button
        EventTrigger trigger = stationButton.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry dragEntry = new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag };
        dragEntry.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
        trigger.triggers.Add(dragEntry);

        EventTrigger.Entry dragEntryMove = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
        dragEntryMove.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(dragEntryMove);

        EventTrigger.Entry dropEntry = new EventTrigger.Entry { eventID = EventTriggerType.EndDrag };
        dropEntry.callback.AddListener((data) => { OnDrop((PointerEventData)data); });
        trigger.triggers.Add(dropEntry);
    }

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

    private void OnBeginDrag(PointerEventData eventData)
    {
        // Make UI group transparent during drag
        uiGroupCanvasGroup.alpha = 0.5f;
    }

    private void OnDrag(PointerEventData eventData)
    {
        // Update the button's position to follow the mouse during drag
        stationButton.transform.position = eventData.position;
    }

    private void OnDrop(PointerEventData eventData)
    {
        // Convert screen position to world position
        Vector3 screenPos = eventData.position;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f; // Ensure the z-position is 0 to place the prefab on the correct plane

        // Convert world position to cell position
        Vector3Int cellPosition = roadTilemap.WorldToCell(worldPos);

        // Calculate the center position of the cell
        Vector3 cellCenterWorldPosition = roadTilemap.GetCellCenterWorld(cellPosition);

        // Instantiate the station prefab at the cell center position
        Instantiate(stationPrefab, cellCenterWorldPosition, Quaternion.identity);

        // Reset the button position
        stationButton.transform.localPosition = Vector3.zero;

        // Restore UI group visibility
        uiGroupCanvasGroup.alpha = 1f;
    }
}