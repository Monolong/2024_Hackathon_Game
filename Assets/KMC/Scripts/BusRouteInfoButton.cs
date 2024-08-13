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
            // ��带 ����Ѵ�.

            // ������ �ø���.
            BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"] += 1;

            // �ؽ�Ʈ�� �����Ѵ�.
            busCountText.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"].ToString();

            // �������� ������ �ø���.
        }
    }

    public void MinusBusCount()
    {
        if (BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"] > 0)
        {
            // ��带 �޴´�.
            // ��尡 ���ڶ�� ����Ѵ�.

            // ������ �ø���.
            BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"] -= 1;

            // �ؽ�Ʈ�� �����Ѵ�.
            busCountText.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"].ToString();

            // �������� ������ ���δ�.
        }
    }

    public void ChangeBusInterval()
    {
        BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busInterval"] = int.Parse(busIntervalComponent.text);

        Debug.Log(BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busInterval"] + "�� ���� �ð� ����");
    }

    public void UpdateUI()
    {
        busIdComponent.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busId"].ToString();

        Debug.Log(buttonIndex + ", " + BusRouteInfo.Instance.busRouteInfo.Count + ", " + BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busId"]);

        busCountText.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"].ToString();
        busIntervalComponent.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busInterval"].ToString();
    }
}
