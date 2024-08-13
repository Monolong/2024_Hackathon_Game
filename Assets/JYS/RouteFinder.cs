//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class RouteFinder : MonoBehaviour
//{
//    Dictionary<int, Dictionary<int, Edge>> graph = new Dictionary<int, Dictionary<int, Edge>>();
//    int numBusRoute = 1;

//    public void InitGraph(Vector3 citizenPosition)
//    {
//        GameObject[] stationObjects = GameObject.FindGameObjectsWithTag("Station");

//        int index = 1;
//        foreach (GameObject stationObject in stationObjects)
//        {
//            Station station = stationObject.GetComponent<Station>();
//            foreach (int busId in station.prevStation.Keys)
//            {
//                graph.[0][station.stationId[busId]] = new Edge(station.transform.position, (int)Vector3.Magnitude(transform.position - station.transform.position), busId);
//                graph[station.stationId[busId]][station.nextStation[busId].stationId[busId]] = new Edge(
//                    station.nextStation[busId].transform.position,
//                    (int) Vector3.Magnitude(station.transform.position - station.nextStation[busId].transform.position),
//                    busId
//                    );
//            }
               
//            foreach (int busId1 in station.prevStation.Keys)
//            {
//                foreach(int busId2 in station.prevStation.Keys)
//                {
//                    if (busId1 != busId2)
//                    {
//                        graph[station.stationId[busId1]][station.stationId[busId2]] = new Edge(
//                            station.transform.position,
//                            1,
//                            busId2
//                            );
//                        graph[station.stationId[busId2]][station.stationId[busId1]] = new Edge(
//                            station.transform.position,
//                            1,
//                            busId1
//                            );
//                    }
//                }
//            }

//            index++;
//        }
//    }

//    public void Dijkstra(int start)
//    {
//        // 노드 수를 V로 가정
//        int V = graph.Keys.Count;

//        // 최단 거리 배열
//        Dictionary<int, int> dist = new Dictionary<int, int>();
//        // 최단 경로 트리 집합
//        HashSet<int> sptSet = new HashSet<int>();

//        // 모든 노드의 초기 거리를 무한대로 설정
//        foreach (var node in graph.Keys)
//        {
//            dist[node] = int.MaxValue;
//        }
//        dist[start] = 0;

//        // 우선순위 큐(최소 힙)를 사용해 가장 작은 거리 노드를 선택
//        SortedSet<(int distance, int node)> pq = new SortedSet<(int, int)>();
//        pq.Add((0, start));

//        while (pq.Count > 0)
//        {
//            var (currentDist, u) = pq.Min;
//            pq.Remove(pq.Min);

//            if (sptSet.Contains(u))
//            {
//                continue;
//            }
//            sptSet.Add(u);

//            if (!graph.ContainsKey(u)) continue;

//            // 현재 노드 u의 모든 이웃 노드를 탐색
//            foreach (var edgeEntry in graph[u])
//            {
//                int v = edgeEntry.Key;
//                Edge edge = edgeEntry.Value;

//                int weight = edge.weight;

//                // 만약 dist[u] + weight < dist[v]라면 dist[v]를 갱신
//                if (!sptSet.Contains(v) && dist[u] + weight < dist[v])
//                {
//                    dist[v] = dist[u] + weight;
//                    pq.Add((dist[v], v));
//                }
//            }
//        }

//        // 최단 경로 출력
//        foreach (var node in dist.Keys)
//        {
//            if (dist[node] == int.MaxValue)
//            {
//                Debug.Log($"Node {node}: Unreachable");
//            }
//            else
//            {
//                Debug.Log($"Distance from Node {start} to Node {node}: {dist[node]}");
//            }
//        }
//    }


//    class Edge
//    {
//        public int destination { get; }
//        public int weight { get; }
//        public int busId { get; }
//        public Vector3 position;

//        public Edge(Vector3 position, int weight, int busId)
//        {
//            this.position = position;
//            this.weight = weight;
//            this.busId = busId;
//        }
//    }
//}