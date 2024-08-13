using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIButton : MonoBehaviour
{
    public void OpenIInventoryUI()
    {
        // ���õ� ���� ���̵� �����Ѵ�.
        int buttonIndex = transform.parent.GetComponent<BusRouteInfoButton>().buttonIndex;
        BusRouteInfo.Instance.selectedBusId = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busId"];

        // ���� �г��� �ݰ� �κ��丮�� ����.
        UIManager2.Instance.TogglePanelVisibility();
        BusRouteInfo.Instance.CloseBusRouteInfoPanel();

        // ���� ���� ǥ���Ѵ�.
        BusRouteInfo.Instance.isEditing = true;
    }
}
