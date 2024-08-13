using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class Citizen : MonoBehaviour
{
    public Node destinationNode;
    public int boardingTime = 0;
    public float happy = 0f;
    public int busId = 0;
    private int startTime = 0;
    private int arriveTime = 0;
    private int totalTime = 0;
    private float movespeed = 5f;
    private Station startStation;
    private Station destinationStation;
    private ObjectPoolManager objPoolManager;
    private Station station;
    private TimeManager timeManager;
    void Start()
    {
        startTime = GetTime();
        FindStartStation();
        FindDestinationStation();
        //StartCoroutine(WaitForBus());
    }

    void Update()
    {
        MoveToStation();
        AddCitizenToStation(busId, this.gameObject); //GetWaitBusId« ø‰«‘
        if(transform.position != destinationNode.transform.position)
        {
            GoDestinationNode();
        }
        if (transform.position == destinationNode.transform.position)
        {
            arriveTime = GetTime();
            CalcurateHappy();
        }
    }

    public void SetDestinationNode(Node RndNode)
    {
        destinationNode = RndNode;
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
        startStation = closestStation;
    }

    public void FindDestinationStation()
    {
        if (destinationNode != null)
        {
            Vector3 DestinationPosition = destinationNode.transform.position;
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
            destinationStation = closestStation;
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
        if (Vector3.Magnitude(startStation.transform.position - transform.position) > Vector3.Magnitude(destinationStation.transform.position - transform.position))
        {
            Vector3 walkVector = destinationStation.transform.position - transform.position;
            transform.position += walkVector.normalized * movespeed * Time.deltaTime;
        }
        Vector3 moveVector = startStation.transform.position - transform.position;
        transform.position += moveVector.normalized * movespeed * Time.deltaTime;
    }

    private void AddCitizenToStation(int busId, GameObject citizen)
    {
        if(transform.position == startStation.transform.position)
        {
            station.waitingCitizens[busId].Enqueue(citizen);
            objPoolManager.ReturnObject(citizen);
        }
    }

    private void GoDestinationNode()
    {
        Vector3 walkVector = destinationNode.transform.position - transform.position;
        transform.position += walkVector.normalized * movespeed * Time.deltaTime;
    }

    private int GetTime()
    {
        int Time = timeManager.hour * 60 + timeManager.minute;

        return Time;
    }
    private int CalcurateTotalTime(int start, int arrive)
    {
        int time = arrive - start;

        return time;
    }

    private void CalcurateHappy()
    {
        totalTime = CalcurateTotalTime(startTime, arriveTime);
        happy = 2 - ((totalTime - boardingTime) * 3 + (boardingTime)) / (totalTime * 2);
    }
    /*private void boardBus(Bus bus)
    {
       
    }*/
}
