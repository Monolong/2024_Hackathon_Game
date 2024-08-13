using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : ObjectPoolManager
{
    // 해당 정류장에 정차하는 버스 이름 (#삭제 예정)
    [SerializeField] private int busId;
    // 다음
    public Dictionary<int, Station> nextStation = new Dictionary<int, Station>();
    public Dictionary<int, Station> prevStation = new Dictionary<int, Station>();
    public bool isGarage = false;

    // 정류장에서 기다리는 시민 배열 (#GameObject -> Citizen 변경 예정)
    public Dictionary<int, Queue<GameObject>> waitingCitizens = new Dictionary<int, Queue<GameObject>>();

    // 화살표 오브젝트
    [SerializeField] protected GameObject arrow;
    [SerializeField] public FixedStationArrow fixedArrow;

    protected virtual void Awake()
    {
        arrow = transform.GetChild(0).gameObject;
        fixedArrow = transform.GetChild(1).GetComponent<FixedStationArrow>();

        arrow.SetActive(false);
    }

    // 마우스가 오브젝트 위에 있고
    private void OnMouseOver()
    {
        Debug.Log("올라온 상태");

        // 우클릭 시
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("우클릭 됨");
            // 다음 정류장 변경 모드로 진입한다.
            StartCoroutine(EnterSetNextStationMode(busId));
        }
    }

    // 정류장 변경 상태로 진입한다.
    public IEnumerator EnterSetNextStationMode(int busId)
    {
        // 화살표를 띄운다.
        arrow.SetActive(true);

        // 다음 정류장에 넣을 객체
        Station selectedNextStation = null;

        // 클릭될 때까지 기다린다.
        while (selectedNextStation == null)
        {
            // 좌클릭 시 해당 타일로 변경한다.
            if (Input.GetMouseButtonDown(0))
            {
                selectedNextStation =  GetClickedStation();
            }

            // 제어권 반환
            yield return null;

            if (Input.GetMouseButtonDown(1))
            {
                // 우클릭 누르면 취소
                arrow.SetActive(false);

                yield break;
            }
        }

        // 성공했을 시 다음 정류장을 등록한다.
        SetNextStation(busId, selectedNextStation);
    }

    // 클릭된 정류장을 반환한다. 없을 시 null을 반환한다.
    public Station GetClickedStation()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0, LayerMask.GetMask("Station"));

        if (hit.collider == null)
        {
            return null;
        }

        return hit.collider.GetComponent<Station>();
    }

    /// <summary>
    /// 다음 정류장을 변경한다.
    /// </summary>
    /// <param name="busId">변경할 노선 정보</param>
    /// <param name="selectedStation">등록할 정류장</param>
    public void SetNextStation(int busId, Station selectedStation)
    {
        Debug.Log("등록 성공");
        // 화살표를 끈다.
        arrow.SetActive(false);

        // 해당 정류장을 등록한다.
        nextStation[busId] = selectedStation;
        // 다음 정류장에도 자기 자신을 등록한다.
        selectedStation.prevStation[busId] = this;

        // 연결된 노선을 표시한다.
        fixedArrow.DrawArrow(transform.position, selectedStation.transform.position);
    }

    /// <summary>
    /// 다음 정류장으로 이동한다.
    /// </summary>
    /// <param name="busId">버스 Id</param>
    /// <returns>해당 버스가 이동할 다음 정류장</returns>
    public Station GetNextStation(int busId)
    {
        return nextStation[busId];
    }

    // 배치 및 삭제한다.
    // 정류장을 배치한다.
    public void PlaceStation()
    {

    }

    // 정류장을 삭제한다.
    public void RemoveStation(int busId)
    {
        Station prev = prevStation[busId];
        Station next = nextStation[busId];

        // 이전 정류장과 다음 정류장을 잇는다.
        prev.nextStation[busId] = next;
        prev.fixedArrow.DrawArrow(prev.transform.position, next.transform.position);

        // 모든 노선 삭제 시 자기 자신을 삭제한다. (#풀링 적용 예정)
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 시민을 정류장에 등록한다. (#GameObject -> Citizen 변경 예정)
    /// </summary>
    /// <param name="busId">시민이 기다리는 버스 Id</param>
    /// <param name="citizen">큐에 등록할 시민 객체</param>
    public void AddCitizenToStation(int busId, GameObject citizen)
    {
        waitingCitizens[busId].Enqueue(citizen);
    }

    // #추가 작업 (가능 시)
    // 노선이 연결되지 않으면 모든 정류장과 화살표를 빨갛게 표시한다.
}
