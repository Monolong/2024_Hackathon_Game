using System.Collections;
using System.Collections.Generic;
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

    // �� ������ �����Ѵ�.
    public void CreateNewBus()
    {
        // ���� ��� �Ŵ����� �� ���� ������ �߰��Ѵ�.
        // �ڱ� �ڽ��� ��� �������� �����Ѵ�.
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

            yield return new WaitForSeconds(busInterval[busId]);
        }
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
