using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusRouteInfoButton : MonoBehaviour
{
    public int buttonIndex;
    public TMP_Text busIdComponent;
    public TMP_Text busCountText;
    public TMP_InputField busIntervalComponent;

    [SerializeField] private Garage garage;

    private void Awake()
    {
        busIdComponent = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        busCountText = transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        busIntervalComponent = transform.GetChild(2).GetComponent<TMP_InputField>();

        garage = FindObjectOfType<Garage>();
    }

    public void PlusBusCount()
    {
        // 골드를 사용한다.
        if (ResourceManager.Instance.SpendMoney(BusRouteInfo.Instance.busPrice) == false)
        {
            // 실패 시 취소
            return;
        }

        // 버스를 추가한다.
        garage.IncreaseBus(BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busId"]);

        // 텍스트를 갱신한다.
        busCountText.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"].ToString();
    }

    public void MinusBusCount()
    {
        // 버스가 없거나 운행 중이라면 취소한다.
        if (garage.DecreaseBus(BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busId"]) == false)
        {
            Debug.LogError("운행 중인 버스가 있거나 버스가 없습니다.");

            return;
        }

        // 골드를 받는다.
        ResourceManager.Instance.EarnMoney(BusRouteInfo.Instance.busPrice);

        // 텍스트를 갱신한다.
        busCountText.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"].ToString();
    }

    public void ChangeBusInterval()
    {
        BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busInterval"] = int.Parse(busIntervalComponent.text);
    }

    public void UpdateUI()
    {
        busIdComponent.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busId"].ToString();

        busCountText.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"].ToString();
        busIntervalComponent.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busInterval"].ToString();
    }
}
