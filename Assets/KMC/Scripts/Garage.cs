using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Garage : Station
{
    #region ����
    // {����, ���� ����} ��ųʸ�
    private Dictionary<int, int> busInterval = new Dictionary<int, int>();
    // ��� �غ� �� ���� ����Ʈ
    [SerializeField] public Dictionary<int, Queue<Bus>> readyBuses = new Dictionary<int, Queue<Bus>>();

    public int currentBusIndex = 100;
    #endregion ����
    public static Garage Instance;

    private void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            enabled = false;
        }

        isGarage = true;
    }

    private void OnMouseOver()
    {
        // ���� ���̸� ��Ŭ�� ��
        if (BusRouteInfo.Instance.isEditing == true && Input.GetMouseButtonDown(1))
        {
            // ���� ������ ���� ���� �����Ѵ�.
            StartCoroutine(EnterSetNextStationMode(BusRouteInfo.Instance.selectedBusId));
        }

        // ���� �� ��Ŭ�� ��
        if (BusRouteInfo.Instance.isEditing == true && Input.GetMouseButtonDown(0))
        {
            // �κ��丮�� ����.
            UIManager2.Instance.OpenPanel();
        }

        // ���� ���� �ƴϰ� ��Ŭ�� ��
        if (BusRouteInfo.Instance.isEditing == false && Input.GetMouseButtonDown(0))
        {
            // �г��� ����.
            BusRouteInfo.Instance.OpenBusRouteInfoPanel();
        }
    }

    // �� ������ �����Ѵ�.
    public void CreateNewBusRoute()
    {
        // ���� ��� �Ŵ����� �� ���� ������ �߰��Ѵ�.
        BusRouteInfo.Instance.AddBusRoute(currentBusIndex, this, 0, 10);

        readyBuses[currentBusIndex] = new Queue<Bus>();

        // ���� ������ �����Ѵ�.
        StartCoroutine(StartBusSystem(currentBusIndex));

        ++currentBusIndex;
    }

    // �������� �����Ѵ�.
    public void RemoveStation(int busId)
    {
        // �ϴ� ���� �� �ϰ� ���Ƶ���.
        Debug.LogError("�������� ������ �� �����ϴ�.");
    }

    /// <summary>
    /// ���� ������ �����Ѵ�.
    /// </summary>
    /// <param name="busId">������ ���� �̸�</param>
    public IEnumerator StartBusSystem(int busId)
    {
        while (true)
        {
            // ����� �������� ���ų�, ���� ���̰ų�, ������ ���ٸ� �۵����� �ʴ´�.
            while (BusRouteInfo.Instance.isEditing == true || nextStation.ContainsKey(busId) == false || readyBuses[busId].Count <= 0)
            {
                yield return null;
            }

            // ���� ���ݸ�ŭ ��ٸ���. (#�ΰ��� �ð����� ���� ����)
            yield return new WaitForSeconds(BusRouteInfo.Instance.busRouteInfo[busId - 100]["busInterval"]);

            // ������ ��߽�Ų��.
            // ���� ���� �� �ִ� ������ ���ٸ�
            if (readyBuses[busId].Count <= 0)
            {
                // �ǳʶڴ�.
                Debug.Log("������ ����.");
                continue;
            }

            Debug.Log(readyBuses[busId].Count);
            // ���� �� �ִٸ� ��߽�Ų��.
            Bus bus = readyBuses[busId].Dequeue();
            // ������ ��߽�Ų��.
            bus.transform.parent = transform;
            bus.transform.localPosition = Vector3.zero;
            bus.InitStation(busId, nextStation[busId]);
            bus.gameObject.SetActive(true);
            StartCoroutine(bus.MoveToStation());
        }
    }

    public void IncreaseBus(int busId)
    {
        // ������ �ø���.
        BusRouteInfo.Instance.busRouteInfo[busId - 100]["busCount"] += 1;

        //Bus bus = GetObject().GetComponent<Bus>();
        Bus bus = Instantiate(prefab, transform).GetComponent<Bus>();

        Station next = this;
        if (nextStation.ContainsKey(busId))
        {
            next = nextStation[busId];
        }

        bus.InitStation(busId, next);
        bus.gameObject.SetActive(false);

        readyBuses[busId].Enqueue(bus);
    }

    public bool DecreaseBus(int busId)
    {
        // ���� ���� ���̶��
        if(readyBuses[busId].Count <= 0)
        {
            // ����Ѵ�. (���� �˸�)
            return false;
        }

        // ������ ���δ�.
        BusRouteInfo.Instance.busRouteInfo[busId - 100]["busCount"] -= 1;
        readyBuses[busId].Dequeue();

        // ���� �˸�
        return true;
    }

    // ���� �뼱�� �����Ѵ�.
    public void ApplyRoute()
    {
        int selectedBusId = BusRouteInfo.Instance.selectedBusId;

        Station lastStation = nextStation[selectedBusId];
        while (lastStation != null && lastStation.isGarage == false)
        {
            if(lastStation.nextStation.ContainsKey(selectedBusId) == false)
            {
                lastStation = null;
                continue;
            }

            lastStation = lastStation.nextStation[selectedBusId];
        }

        // ���������� ����� �������� ���ƿ��� ���� ���
        if (lastStation == null)
        {
            // ���ư� �� ����.
            return;
        }

        // ���������� ����� �������� ���ƿ� ���
        if (lastStation != null && lastStation.isGarage == true)
        {
            // �����Ѵ�.
            lastStation = this;
            fixedArrow.gameObject.SetActive(false);
            lastStation = nextStation[selectedBusId];

            while (lastStation != null && lastStation.isGarage == false)
            {
                lastStation.fixedArrow.gameObject.SetActive(false);
                lastStation = lastStation.nextStation[selectedBusId];
            }

            BusRouteInfo.Instance.isEditing = false;
            BusRouteInfo.Instance.CloseBusRouteInfoPanel();
            UIManager2.Instance.TogglePanelVisibility();
        }
    }

    /// <summary>
    /// ���� �������� �����Ѵ�.
    /// </summary>
    /// <param name="busId">������ �뼱 ����</param>
    /// <param name="selectedStation">����� ������</param>
    public void SetNextStation(int busId, Station selectedStation)
    {
        // �ش� �������� ����Ѵ�.
        nextStation[busId] = selectedStation;

        // ���� �������� �������� �ʴ´�.
        prevStation[busId] = null;
    }
}
