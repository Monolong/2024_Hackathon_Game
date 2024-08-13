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

    protected override void Awake()
    {
        base.Awake();

        isGarage = true;
    }

    private void OnMouseOver()
    {
        // ��Ŭ�� ��
        if (Input.GetMouseButtonDown(0))
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

        ++currentBusIndex;

        // ���� ������ �����Ѵ�.
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
            // ���� ���ݸ�ŭ ��ٸ���. (#�ΰ��� �ð����� ���� ����)
            yield return new WaitForSeconds(busInterval[busId]);

            // ������ ��߽�Ų��.
            // ���� ���� �� �ִ� ������ ���ٸ�
            if (readyBuses.Count <= 0)
            {
                // �ǳʶڴ�.
                continue;
            }

            // ���� �� �ִٸ� ��߽�Ų��.
            Bus bus = readyBuses[busId].Dequeue();
            // ������ ��߽�Ų��.
            StartCoroutine(bus.MoveToStation());
        }
    }

    public void IncreaseBus(int busId)
    {
        // ������ �ø���.
        BusRouteInfo.Instance.busRouteInfo[busId - 100]["busCount"] += 1;

        Bus bus = GetObject().GetComponent<Bus>();

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
