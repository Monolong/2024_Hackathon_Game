using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BusRouteInfo : ObjectPoolManager
{
    public List<Dictionary<string, int>> busRouteInfo = new List<Dictionary<string, int>>();
    [SerializeField] float width;
    [SerializeField] bool isOpened = false;

    public GameObject buttonParentObject;
    public RectTransform buttonParent;

    RectTransform rectTransform;

    public int busPrice = 5000;

    public bool isEditing = false;
    public bool isConnecting = false;

    public int selectedBusId;

    public static BusRouteInfo Instance { get; set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            enabled = false;
        }

        rectTransform = GetComponent<RectTransform>();
        buttonParent = buttonParentObject.GetComponent<RectTransform>();
        width = rectTransform.rect.width;
    }

    public void AddBusRoute(int busId, Garage garage, int busCount, int interval)
    {
        busRouteInfo.Add(new Dictionary<string, int>
        {
            { "busId", busId },
            { "busCount", busCount },
            { "readyBusCount", busCount },
            { "busInterval", interval },
        });

        GameObject obj = GetObject();
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.parent = buttonParent;
        rectTransform.localScale = Vector3.one;

        BusRouteInfoButton busRouteInfoButton = obj.GetComponent<BusRouteInfoButton>();
        busRouteInfoButton.buttonIndex = busId - 100;
        busRouteInfoButton.UpdateUI();
    }

    public void OpenBusRouteInfoPanel()
    {
        if (isOpened)
        {
            return;
        }

        isOpened = true;
        rectTransform.DOAnchorPosX(0, 1f);
    }


    public void CloseBusRouteInfoPanel()
    {
        if (isOpened == false)
        {
            return;
        }

        isOpened = false;
        rectTransform.DOAnchorPosX(width, 1f);
    }
}
