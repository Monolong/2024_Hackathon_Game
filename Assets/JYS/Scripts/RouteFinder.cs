using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteFinder : MonoBehaviour
{
    Dictionary<int, HashSet<Station>> stations = new Dictionary<int, HashSet<Station>>();
    Citizen citizen;
    public void InitRouteFinder(Citizen citizen)
    {
        this.citizen = citizen;
        GameObject[] tempStationGameObjects = GameObject.FindGameObjectsWithTag("Station");
        
        foreach(GameObject tempStationGameObject in tempStationGameObjects)
        {
            Station tempStation = tempStationGameObject.GetComponent<Station>();
            foreach (int busId in tempStation.nextStation.Keys)
            {
                try
                {
                    if (stations[busId] == null)
                        print("zz");
                }
                catch
                {
                    stations.Add(busId, new HashSet<Station>());
                }

                stations[busId].Add(tempStation);
            }
        }
    }

    public Station[] FindRoute(Vector3 startPosition, Vector3 endPosition)
    {
        
        Station[] result = new Station[2];
        try
        {
            Station minStartDistanceStation;
            Station minEndDistanceStation;
            int globalMinStartDistance = int.MaxValue / 2;
            int globalMinEndDistance = int.MaxValue / 2;
            
            foreach (int busId in stations.Keys)
            {
                int minStartDistance = int.MaxValue;
                int minEndDistance = int.MaxValue;
                minStartDistanceStation = null;
                minEndDistanceStation = null;
                foreach (Station station in stations[busId])
                {
                    if (minStartDistance > Vector3.Magnitude(station.transform.position - startPosition))
                    {
                        minStartDistance = (int)Vector3.Magnitude(station.transform.position - startPosition);
                        minStartDistanceStation = station;
                    }
                    print(Vector3.Magnitude(station.transform.position - endPosition));
                    if (minEndDistance > Vector3.Magnitude(station.transform.position - endPosition))
                    {
                        minEndDistance = (int)Vector3.Magnitude(station.transform.position - endPosition);
                        minEndDistanceStation = station;
                    }
                }


                if (globalMinEndDistance + globalMinStartDistance > minStartDistance + minEndDistance)
                {
                    if (minStartDistanceStation != null && minEndDistanceStation != null)
                    {
                        globalMinEndDistance = minEndDistance;
                        globalMinStartDistance = minStartDistance;
                        citizen.startStation = minStartDistanceStation;
                        citizen.destinationStation = minEndDistanceStation;
                    }
                    
                }
            }
            return result;
        }
        catch
        {
            throw new Exception("route not found");
        }
    
    }
}
