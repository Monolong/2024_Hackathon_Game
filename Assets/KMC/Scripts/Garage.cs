using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Garage : Station
{
    #region 변수
    // {버스, 배차 간격} 딕셔너리
    private Dictionary<int, int> busInterval = new Dictionary<int, int>();
    #endregion 변수

    private void Awake()
    {
        isGarage = true;
    }

    private void OnMouseOver()
    {
        // 마우스 작업도 막는다.
    }

    // 새 버스를 생성한다.
    public void CreateNewBus()
    {
        // 버스 경로 매니저에 새 버스 정보를 추가한다.
        // 자기 자신을 출발 차고지로 설정한다.
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
            // 버스를 출발시킨다.

            // 배차 간격만큼 기다린다.
            yield return new WaitForSeconds(busInterval[busId]);
        }
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

    /// <summary>
    /// 버스 배차 간격을 변경한다.
    /// </summary>
    /// <param name="busId">변경할 버스 이름</param>
    /// <param name="time">변경할 시간</param>
    public void ModifyBusInterval(int busId, int time)
    {
        busInterval[busId] = time;
    }
}
