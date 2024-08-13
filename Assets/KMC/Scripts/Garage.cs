using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Garage : Station
{
    #region 변수
    // {버스, 배차 간격} 딕셔너리
    private Dictionary<int, int> busInterval = new Dictionary<int, int>();
    // 출발 준비 된 버스 리스트
    [SerializeField] public Dictionary<int, Queue<Bus>> readyBuses = new Dictionary<int, Queue<Bus>>();

    public int currentBusIndex = 100;
    #endregion 변수

    protected override void Awake()
    {
        base.Awake();

        isGarage = true;
    }

    private void OnMouseOver()
    {
        // 좌클릭 시
        if (Input.GetMouseButtonDown(0))
        {
            // 패널을 연다.
            BusRouteInfo.Instance.OpenBusRouteInfoPanel();
        }
    }

    // 새 버스를 생성한다.
    public void CreateNewBusRoute()
    {
        // 버스 경로 매니저에 새 버스 정보를 추가한다.
        BusRouteInfo.Instance.AddBusRoute(currentBusIndex, this, 0, 10);

        readyBuses[currentBusIndex] = new Queue<Bus>();

        ++currentBusIndex;

        // 버스 운행을 시작한다.
    }

    // 정류장을 삭제한다.
    public void RemoveStation(int busId)
    {
        // 일단 삭제 못 하게 막아두자.
        Debug.LogError("차고지는 삭제할 수 없습니다.");
    }

    /// <summary>
    /// 버스 운행을 시작한다.
    /// </summary>
    /// <param name="busId">운행할 버스 이름</param>
    public IEnumerator StartBusSystem(int busId)
    {
        while (true)
        {
            // 배차 간격만큼 기다린다. (#인게임 시간으로 변경 예정)
            yield return new WaitForSeconds(busInterval[busId]);

            // 버스를 출발시킨다.
            // 만약 보낼 수 있는 버스가 없다면
            if (readyBuses.Count <= 0)
            {
                // 건너뛴다.
                continue;
            }

            // 보낼 수 있다면 출발시킨다.
            Bus bus = readyBuses[busId].Dequeue();
            // 버스를 출발시킨다.
            StartCoroutine(bus.MoveToStation());
        }
    }

    public void IncreaseBus(int busId)
    {
        // 버스를 늘린다.
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
        // 전부 운행 중이라면
        if(readyBuses[busId].Count <= 0)
        {
            // 취소한다. (실패 알림)
            return false;
        }

        // 버스를 줄인다.
        BusRouteInfo.Instance.busRouteInfo[busId - 100]["busCount"] -= 1;
        readyBuses[busId].Dequeue();

        // 성공 알림
        return true;
    }

    /// <summary>
    /// 다음 정류장을 변경한다.
    /// </summary>
    /// <param name="busId">변경할 노선 정보</param>
    /// <param name="selectedStation">등록할 정류장</param>
    public void SetNextStation(int busId, Station selectedStation)
    {
        // 해당 정류장을 등록한다.
        nextStation[busId] = selectedStation;

        // 이전 정류장은 존재하지 않는다.
        prevStation[busId] = null;
    }
}
