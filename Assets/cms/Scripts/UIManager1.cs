using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Added to use TextMeshPro

public class UIManager1 : MonoBehaviour
{
    public TextMeshProUGUI timeText; // Current time text (Text -> TextMeshProUGUI)
    public TextMeshProUGUI moneyText; // Current money text (Text -> TextMeshProUGUI)
    public TextMeshProUGUI satisfactionText; // Current satisfaction text (Text -> TextMeshProUGUI)

    private TimeManager timeManager;
    private ResourceManager resourceManager;

    void Start()
    {
        // Find TimeManager and ResourceManager components
        timeManager = FindObjectOfType<TimeManager>();
        resourceManager = FindObjectOfType<ResourceManager>();

        // Initial UI update
        UpdateUI();
    }

    void Update()
    {
        // Update UI every frame
        UpdateUI();
    }

    void UpdateUI()
    {
        // Update text with current time from TimeManager
        timeText.text = "시간: " + timeManager.Hours.ToString("D2") + ":" + timeManager.Minutes.ToString("D2");

        // Update text with current money and satisfaction from ResourceManager
        moneyText.text = "돈: $" + resourceManager.Money.ToString();
        satisfactionText.text = "만족도: " + resourceManager.Satisfaction.ToString("F1") + "%";
    }
}