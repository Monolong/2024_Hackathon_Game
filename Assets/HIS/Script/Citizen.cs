using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class Citizen : MonoBehaviour
{
    public Node DestinationNode;
    public int BoardingTime = 0;
    public float Happy = 0f;
    private int StartTime = 0;
    private int ArriveTime = 0;
    private int TotalTime = 0;
    private float movespeed = 5f;
    private Station StartStation;
    private Station DestinationStation;
    private ObjectPoolManager objPoolManager;
    private Station station;
    private TimeManager timeManager;
    void Start()
    {
        StartTime = GetTime();
        FindStartStation();
        FindDestinationStation();
        //StartCoroutine(WaitForBus());
    }

    void Update()
    {
        MoveToStation();
        AddCitizenToStation(busId, this); //GetWaitBusId« ø‰«‘
        if(transform.position != DestinationNode.transform.position)
        {
            GoDestinationNode();
        }
        if (transform.position = DestinationNode.transform.position)
        {
            ArriveTime = GetTime();
            CalcurateHappy();
        }
    }

    public void SetDestinationNode(Node RndNode)
    {
        DestinationNode = RndNode;
    }

    public void FindStartStation()
    {
        Vector3 playerPosition = transform.position;
        GameObject[] allStations = GameObject.FindGameObjectsWithTag("Station");
        Station closestStation = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject stationObject in allStations)
        {
            Station station = stationObject.GetComponent<Station>();
            if (station != null)
            {
                float distance = Vector3.Distance(playerPosition, station.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestStation = station;
                }
            }
        }
        StartStation = closestStation;
    }

    public void FindDestinationStation()
    {
        if (DestinationNode != null)
        {
            Vector3 DestinationPosition = DestinationNode.transform.position;
            GameObject[] allStations = GameObject.FindGameObjectsWithTag("Station");
            Station closestStation = null;
            float closestDistance = Mathf.Infinity;
            foreach (GameObject stationObject in allStations)
            {
                Station station = stationObject.GetComponent<Station>();
                if (station != null)
                {
                    float distance = Vector3.Distance(DestinationPosition, station.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestStation = station;
                    }
                }
            }
            DestinationStation = closestStation;
        }
    }

    /*private IEnumerator WaitForBus()
    {
        while (true)
        {
            if (StartStation != null && DestinationStation != null)
            {
                Bus bus = FindBus();
                if (bus != null && bus.IsGoingTo(StartStation, DestinationStation))
                {
                    yield return new WaitUntil(() => Vector3.Distance(transform.position, StartStation.transform.position) < 1.0f);
                    if (bus.IsAtStation(StartStation) && bus.IsGoingTo(DestinationStation))
                    {
                        boardBus(bus);
                    }
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    private Bus FindBus()
    {
        GameObject[] allBuses = GameObject.FindGameObjectsWithTag("Bus");
        foreach (GameObject busObject in allBuses)
        {
            Bus bus = busObject.GetComponent<Bus>();
            if (bus != null)
            {
                return bus;
            }
        }
        return null;
    }*/

    private void MoveToStation()
    {
        if (Vector3.Magnitude(StartStation.transform.position - transform.position) > Vector3.Magnitude(DestinationStation.transform.position - transform.position))
        {
            Vector3 WalkVector = DestinationStation.transform.position - transform.position;
            transform.position += WalkVector.normalized * movespeed * Time.deltaTime;
        }
        Vector3 moveVector = StartStation.transform.position - transform.position;
        transform.position += moveVector.normalized * movespeed * Time.deltaTime;
    }

    private void AddCitizenToStation(int busId, GameObject citizen)
    {
        if(transform.position == StartStation.transform.position)
        {
            station.waitingCitizens[busId].Enqueue(citizen);
            objPoolManager.ReturnObject(citizen);
        }
    }

    private void GoDestinationNode()
    {
        Vector3 WalkVector = DestinationNode.transform.position - transform.position;
        transform.position += WalkVector.normalized * movespeed * Time.deltaTime;
    }

    private int GetTime()
    {
        int Time = timeManager.hour * 60 + timeManager.minute;

        return Time;
    }
    private int CalcurateTotalTime(int Start, int Arrive)
    {
        int Time = Arrive - Start;

        return Time;
    }

    private void CalcurateHappy()
    {
        TotalTime = CalcurateTotalTime(StartTime, ArriveTime);
        Happy = 2 - ((TotalTime - BoardingTime) * 3 + (BoardingTime)) / (TotalTime * 2);
    }
    /*private void boardBus(Bus bus)
    {
       
    }*/
}
