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
    public int fee = 1500;
    private int startTime = 0;
    private int arriveTime = 0;
    private int totalTime = 0;
    private float movespeed = 0.4f;
    private TimeManager timeManager;

    public bool pooFlag = false;
    void Start()
    {
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        startTime = GetTime();
        FindRoute();
    }

    void Update()
    {
        if(startStation == null || destinationStation == null)
        {
            GoDestinationNode();
            if (Vector3.Magnitude(transform.position - destinationNode.transform.position) < 1)
            {
                arriveTime = GetTime();
                gameObject.SetActive(false);
                CalcurateHappy();
            }
        }
        else
        {
            if (!pooFlag)
                MoveToStation();
            AddCitizenToStation(busId, this);
            if (transform.position != destinationNode.transform.position && pooFlag)
            {
                GoDestinationNode();
            }
            if (Vector3.Magnitude(transform.position - destinationNode.transform.position) < 1)
            {
                arriveTime = GetTime();
                gameObject.SetActive(false);
                CalcurateHappy();
            }
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
        routeFinder.FindRoute(transform.position, destinationNode.transform.position);
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

    private void AddCitizenToStation(int busId, Citizen citizen)
    {
        if((Vector3.Magnitude(transform.position - startStation.transform.position)) < 1)
        {
            try
            {
                startStation.waitingCitizens[busId]
                .Enqueue(citizen);
            } catch
            {
                startStation.waitingCitizens[busId] = new Queue<Citizen>();
                startStation.waitingCitizens[busId].Enqueue(citizen);
            }
            startStation.waitingCitizens[busId]
.Enqueue(citizen);

            citizen.gameObject.SetActive(false);
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
        if(start == arrive)
        {
            return 1;
        }
        return time;
    }

    private void CalcurateHappy()
    {
        totalTime = CalcurateTotalTime(startTime, arriveTime);
        happy = 2 - ((totalTime - boardingTime) * 3 + (boardingTime)) / (totalTime * 2);
    }

    public void PaidBusPrice()
    {
        ResourceManager.Instance.EarnMoney(fee);
    }
}
