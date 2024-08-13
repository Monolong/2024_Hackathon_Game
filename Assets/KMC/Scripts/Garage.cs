using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Garage : Station
{
    #region ����
    // {����, ���� ����} ��ųʸ�
    private Dictionary<int, int> busInterval = new Dictionary<int, int>();
    #endregion ����

    private void Awake()
    {
        isGarage = true;
    }

    private void OnMouseOver()
    {
        // ���콺 �۾��� ���´�.
    }

    // �� ������ �����Ѵ�.
    public void CreateNewBus()
    {
        // ���� ��� �Ŵ����� �� ���� ������ �߰��Ѵ�.
        // �ڱ� �ڽ��� ��� �������� �����Ѵ�.
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
            // ������ ��߽�Ų��.

            // ���� ���ݸ�ŭ ��ٸ���.
            yield return new WaitForSeconds(busInterval[busId]);
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

    /// <summary>
    /// ���� ���� ������ �����Ѵ�.
    /// </summary>
    /// <param name="busId">������ ���� �̸�</param>
    /// <param name="time">������ �ð�</param>
    public void ModifyBusInterval(int busId, int time)
    {
        busInterval[busId] = time;
    }
}
