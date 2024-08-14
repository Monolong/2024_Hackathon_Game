using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bus : MonoBehaviour
{
    [SerializeField] Station nextStation;
    List<Citizen> citizens = new List<Citizen>();
    PathFinder.PathFinder pathFinder;
    MapLoader mapLoader;
    int[,] map;
    Vector3Int mapOffset;
    Vector3 deltaPosition = new Vector3();
    int busId = 0;

    int maxNumCitizen = 5;
    int numCitizen = 0;
    float moveFrame = 0.1f;
    float speed = 2f;
    float waitTimeOnStation = 1f;

    public void InitStation(int busId, Station nextStation)
    {
        this.busId = busId;
        this.nextStation = nextStation;
    }

    private void Start()
    {
        mapLoader = GameObject.Find("BusPathManager").GetComponent<MapLoader>();
        map = mapLoader.GetMap();
        mapOffset = mapLoader.GetMapOffset();
        pathFinder = new PathFinder.PathFinder(map, mapOffset);
    }

    public IEnumerator MoveToStation()
    {
        mapLoader = GameObject.Find("BusPathManager").GetComponent<MapLoader>();
        map = mapLoader.GetMap();
        mapOffset = mapLoader.GetMapOffset();
        pathFinder = new PathFinder.PathFinder(map, mapOffset);

        PathFinder.Node startNode = new PathFinder.Node(true);
        PathFinder.Node endNode = new PathFinder.Node(true);
        startNode.SetPosition(transform.position.x, transform.position.y);
        endNode.SetPosition(nextStation.transform.position.x, nextStation.transform.position.y);

        List<PathFinder.Node> path = pathFinder.FindPath(startNode, endNode);

        foreach(PathFinder.Node node in path)
        {
            float deltaX = node.X - transform.position.x;
            float deltaY = node.Y - transform.position.y;
            deltaPosition.Set(deltaX, deltaY, 0);
            deltaX = Mathf.Abs(deltaX);
            deltaY = Mathf.Abs(deltaY);

            float angle = Mathf.Atan2( - deltaPosition.y,  - deltaPosition.x) * Mathf.Rad2Deg - Mathf.PI;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 800 * Time.deltaTime);
            transform.rotation =  Quaternion.Euler(0, 0, angle);

            deltaPosition.Normalize();

            while (deltaX > 0 || deltaY > 0)
            {
                deltaX -= Mathf.Abs(deltaPosition.x * speed * moveFrame);
                deltaY -= Mathf.Abs(deltaPosition.y * speed * moveFrame);
                transform.position += (deltaPosition * speed * moveFrame);
                yield return new WaitForSeconds(moveFrame);
            }
            transform.position = new Vector3(node.X, node.Y);
        }

        if (!nextStation.isGarage)
        {
            yield return new WaitForSeconds(waitTimeOnStation);

            DropCitizens();

            GiveCitizensRide();

            nextStation = nextStation.GetNextStation(busId);
            StartCoroutine(MoveToStation());
        } else
        {
            Garage.Instance.readyBuses[busId].Enqueue(this);
            gameObject.SetActive(false);
        }
    }
    private void DropCitizens()
    {
        List<Citizen> citizensToDrop = new List<Citizen>();
        foreach(Citizen citizen in citizens)
        {
            if (citizen.destinationStation.Equals(nextStation))
            {
                citizensToDrop.Add(citizen);
            }
        }

        foreach(Citizen citizen in citizensToDrop)
        {
            citizens.Remove(citizen);
            citizen.transform.position = transform.position;
            citizen.gameObject.SetActive(true);
            citizen.pooFlag = true;
        }
    
    }

    private void GiveCitizensRide()
    {
        try
        {
            while (maxNumCitizen > numCitizen)
            {
                Citizen citizen = nextStation.waitingCitizens[busId].Dequeue();
                citizens.Add(citizen);
                citizen.PaidBusPrice();
            }
        } catch { }
    }
}


