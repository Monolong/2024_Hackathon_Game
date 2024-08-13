using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Node : IComparable<Node>
{
    public int X;
    public int Y;
    public bool IsWalkable;
    public int GCost;
    public int HCost;
    public Node Parent;

    public int FCost => GCost + HCost;

    public void SetPosition(float x, float y)
    {
        X = (int)Math.Round(x); Y = (int)Math.Round(y);
    }

    public Node(int x, int y, bool isWalkable)
    {
        X = x;
        Y = y;
        IsWalkable = isWalkable;
    }

    public Node(bool isWalkable)
    {
        IsWalkable = isWalkable;
    }

    public int CompareTo(Node other)
    {
        int compare = FCost.CompareTo(other.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(other.HCost);
        }
        return compare;
    }

    public override bool Equals(object obj)
    {
        if (obj is Node other)
        {
            return X == other.X && Y == other.Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (X, Y).GetHashCode();
    }
}

public class PathFinder
{
    private readonly int[,] map;
    private readonly int width;
    private readonly int height;
    private Vector3Int mapOffset;
    private List<Tuple<int, int>> directions;
    public int maxComputing = 10000;

    private Node[,] nodes;
    public PathFinder(int[,] map, Vector3Int mapOffset)
    {
        this.map = map;
        this.mapOffset = mapOffset;
        width = map.GetLength(0);
        height = map.GetLength(1);
        nodes = new Node[width, height];
    }

    public List<Node> FindPath(Node startNode, Node endNode)
    {
        int computeCount = 0;

        SortedSet<Node> openSet = new SortedSet<Node>() { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();

        startNode.GCost = 0;
        startNode.HCost = GetDistance(startNode, endNode);

        while (openSet.Count > 0 && computeCount < maxComputing)
        {
            computeCount++;
            Node currentNode = openSet.Min;
            if (!currentNode.IsWalkable)
            {
                throw new Exception("creature not available");
            }

            openSet.Remove(openSet.Min);
            closedSet.Add(currentNode);

            if (currentNode.Equals(endNode)) // path found
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (neighbor == null || !neighbor.IsWalkable || closedSet.Contains(neighbor))
                {
                    continue;
                }
                int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = newMovementCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, endNode);
                    neighbor.Parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        throw new Exception("Path not Found");
    }


    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.X + x;
                int checkY = node.Y + y;

                if (checkX - mapOffset.x >= 0 && checkX - mapOffset.x < width && checkY - mapOffset.y >= 0 && checkY - mapOffset.y < height)
                {
                    neighbors.Add(GetNode(checkX, checkY));
                }
            }
        }
        return neighbors;
    }

    private Node GetNode(int x, int y)
    {
        Node tempNode;
        if (this.nodes[x - mapOffset.x, y - mapOffset.y] == null)
        {
            tempNode = new Node(x, y, map[x - mapOffset.x, y - mapOffset.y] == 1);

            this.nodes[x - mapOffset.x, y - mapOffset.y] = tempNode;
        }
        tempNode = this.nodes[x - mapOffset.x, y - mapOffset.y];

        return tempNode;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        return path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Math.Abs(nodeA.X - nodeB.X);
        int dstY = Math.Abs(nodeA.Y - nodeB.Y);
        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
