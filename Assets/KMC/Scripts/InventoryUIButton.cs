using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIButton : MonoBehaviour
{
    UIManager2 uiManager2;

    private void Awake()
    {
        uiManager2 = FindObjectOfType<UIManager2>();
    }

    public void OpenIInventoryUI()
    {
        uiManager2.TogglePanelVisibility();
        BusRouteInfo.Instance.CloseBusRouteInfoPanel();
    }
}
