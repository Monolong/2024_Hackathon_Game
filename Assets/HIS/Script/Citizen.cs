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
    public Station startStation;
    public Station destinationStation;
    private int startTime = 0;
    private int arriveTime = 0;
    private int totalTime = 0;
    private float movespeed = 5f;
    private TimeManager timeManager;
    void Start()
    {
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        startTime = GetTime();
        FindRoute();
    }

    void Update()
    {
        MoveToStation();
        AddCitizenToStation(busId, this.gameObject);
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

    public void FindRoute()
    {
        RouteFinder routeFinder = new RouteFinder();
        routeFinder.InitRouteFinder(this);
        routeFinder.FindRoute();
    }

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
            startStation.waitingCitizens[busId].Enqueue(citizen);
            citizen.SetActive(false);
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
}
