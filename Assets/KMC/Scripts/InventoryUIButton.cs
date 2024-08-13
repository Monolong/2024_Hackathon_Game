using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIButton : MonoBehaviour
{
    public void OpenIInventoryUI()
    {
        // 선택된 버스 아이디를 전달한다.
        int buttonIndex = transform.parent.GetComponent<BusRouteInfoButton>().buttonIndex;
        BusRouteInfo.Instance.selectedBusId = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busId"];

        // 정보 패널은 닫고 인벤토리는 연다.
        UIManager2.Instance.TogglePanelVisibility();
        BusRouteInfo.Instance.CloseBusRouteInfoPanel();

        // 수정 중을 표시한다.
        BusRouteInfo.Instance.isEditing = true;
    }
}
