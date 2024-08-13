using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusRouteInfoButton : MonoBehaviour
{
    public int buttonIndex;
    public TMP_Text busIdComponent;
    public TMP_Text busCountText;
    public TMP_InputField busIntervalComponent;

    private void Awake()
    {
        busIdComponent = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        busCountText = transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        busIntervalComponent = transform.GetChild(2).GetComponent<TMP_InputField>();
    }

    public void PlusBusCount()
    {
        if (BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"] < 3)
        {
            // 골드를 사용한다.

            // 버스를 늘린다.
            BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"] += 1;

            // 텍스트를 갱신한다.
            busCountText.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"].ToString();

            // 차고지의 버스를 늘린다.
        }
    }

    public void MinusBusCount()
    {
        if (BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"] > 0)
        {
            // 골드를 받는다.
            // 골드가 모자라면 취소한다.

            // 버스를 늘린다.
            BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"] -= 1;

            // 텍스트를 갱신한다.
            busCountText.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"].ToString();

            // 차고지의 버스를 줄인다.
        }
    }

    public void ChangeBusInterval()
    {
        BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busInterval"] = int.Parse(busIntervalComponent.text);

        Debug.Log(BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busInterval"] + "로 배차 시간 변경");
    }

    public void UpdateUI()
    {
        busIdComponent.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busId"].ToString();

        Debug.Log(buttonIndex + ", " + BusRouteInfo.Instance.busRouteInfo.Count + ", " + BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busId"]);

        busCountText.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"].ToString();
        busIntervalComponent.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busInterval"].ToString();
    }
}
