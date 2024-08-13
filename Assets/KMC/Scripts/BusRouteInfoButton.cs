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
        // ��带 ����Ѵ�.
        if (ResourceManager.Instance.SpendMoney(BusRouteInfo.Instance.busPrice) == false)
        {
            // ���� �� ���
            return;
        }

        // ������ �߰��Ѵ�.
        garage.IncreaseBus(BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busId"]);

        // �ؽ�Ʈ�� �����Ѵ�.
        busCountText.text = BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busCount"].ToString();
    }

    public void MinusBusCount()
    {
        // ������ ���ų� ���� ���̶�� ����Ѵ�.
        if (garage.DecreaseBus(BusRouteInfo.Instance.busRouteInfo[buttonIndex]["busId"]) == false)
        {
            Debug.LogError("���� ���� ������ �ְų� ������ �����ϴ�.");

            return;
        }

        // ��带 �޴´�.
        ResourceManager.Instance.EarnMoney(BusRouteInfo.Instance.busPrice);

        // �ؽ�Ʈ�� �����Ѵ�.
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
