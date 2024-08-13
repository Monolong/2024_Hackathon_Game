using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummy;

[System.Serializable]
public struct DestinationInfo
{
    public Node node;
    public List<HourProbInfo> hourProbList;
}

[System.Serializable]
public struct HourProbInfo
{
    public int hour;
    public float probabillity;
}

public class Node : MonoBehaviour
{
    public List<Citizen> citizenList;
    public List<DestinationInfo> possibleDestinationList;

    public void ProduceCitizen()
    {
        Instantiate(gameObject);
    }

}
