using System.Collections;
using System.Collections.Generic;
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

    // 새 버스를 생성한다.
    public void CreateNewBus()
    {
        // 버스 경로 매니저에 새 버스 정보를 추가한다.
        // 자기 자신을 출발 차고지로 설정한다.
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

            yield return new WaitForSeconds(busInterval[busId]);
        }
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
