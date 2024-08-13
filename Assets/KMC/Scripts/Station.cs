using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    // �ش� �����忡 �����ϴ� ���� �̸� (#���� ����)
    [SerializeField] private int busId;
    // ����
    public Dictionary<int, Station> nextStation = new Dictionary<int, Station>();
    public Dictionary<int, Station> prevStation = new Dictionary<int, Station>();
    public bool isGarage = false;

    // �����忡�� ��ٸ��� �ù� �迭 (#GameObject -> Citizen ���� ����)
    public Dictionary<int, Queue<Citizen>> waitingCitizens = new Dictionary<int, Queue<Citizen>>();

    // ȭ��ǥ ������Ʈ
    [SerializeField] protected GameObject arrow;
    [SerializeField] public FixedStationArrow fixedArrow;

    protected virtual void Awake()
    {
        arrow = transform.GetChild(0).gameObject;
        fixedArrow = transform.GetChild(1).GetComponent<FixedStationArrow>();

        arrow.SetActive(false);
    }

    // ���콺�� ������Ʈ ���� �ְ�
    private void OnMouseOver()
    {
        Debug.Log("�ö�� ����");

        // ��Ŭ�� ��
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("��Ŭ�� ��");
            // ���� ������ ���� ���� �����Ѵ�.
            StartCoroutine(EnterSetNextStationMode(busId));
        }
    }

    // ������ ���� ���·� �����Ѵ�.
    public IEnumerator EnterSetNextStationMode(int busId)
    {
        // ȭ��ǥ�� ����.
        arrow.SetActive(true);

        // ���� �����忡 ���� ��ü
        Station selectedNextStation = null;

        // Ŭ���� ������ ��ٸ���.
        while (selectedNextStation == null)
        {
            // ��Ŭ�� �� �ش� Ÿ�Ϸ� �����Ѵ�.
            if (Input.GetMouseButtonDown(0))
            {
                selectedNextStation =  GetClickedStation();
            }

            // ����� ��ȯ
            yield return null;
        }

        // �������� �� ���� �������� ����Ѵ�.
        SetNextStation(busId, selectedNextStation);
    }

    // Ŭ���� �������� ��ȯ�Ѵ�. ���� �� null�� ��ȯ�Ѵ�.
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
    /// ���� �������� �����Ѵ�.
    /// </summary>
    /// <param name="busId">������ �뼱 ����</param>
    /// <param name="selectedStation">����� ������</param>
    public void SetNextStation(int busId, Station selectedStation)
    {
        Debug.Log("��� ����");
        // ȭ��ǥ�� ����.
        arrow.SetActive(false);

        // �ش� �������� ����Ѵ�.
        nextStation[busId] = selectedStation;
        // ���� �����忡�� �ڱ� �ڽ��� ����Ѵ�.
        selectedStation.prevStation[busId] = this;

        // ����� �뼱�� ǥ���Ѵ�.
        fixedArrow.DrawArrow(transform.position, selectedStation.transform.position);
    }

    /// <summary>
    /// ���� ���������� �̵��Ѵ�.
    /// </summary>
    /// <param name="busId">���� Id</param>
    /// <returns>�ش� ������ �̵��� ���� ������</returns>
    public Station GetNextStation(int busId)
    {
        return nextStation[busId];
    }

    // ��ġ �� �����Ѵ�.
    // �������� ��ġ�Ѵ�.
    public void PlaceStation()
    {

    }

    // �������� �����Ѵ�.
    public void RemoveStation(int busId)
    {
        Station prev = prevStation[busId];
        Station next = nextStation[busId];

        // ���� ������� ���� �������� �մ´�.
        prev.nextStation[busId] = next;
        prev.fixedArrow.DrawArrow(prev.transform.position, next.transform.position);

        // ��� �뼱 ���� �� �ڱ� �ڽ��� �����Ѵ�. (#Ǯ�� ���� ����)
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �ù��� �����忡 ����Ѵ�. (#GameObject -> Citizen ���� ����)
    /// </summary>
    /// <param name="busId">�ù��� ��ٸ��� ���� Id</param>
    /// <param name="citizen">ť�� ����� �ù� ��ü</param>
    public void AddCitizenToStation(int busId, Citizen citizen)
    {
        waitingCitizens[busId].Enqueue(citizen);
    }

    // #�߰� �۾� (���� ��)
    // �뼱�� ������� ������ ��� ������� ȭ��ǥ�� ������ ǥ���Ѵ�.
}
