using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummy;

[System.Serializable]
public struct DestinationInfo
{
    public int hourStart;
    public int hourEnd;
    public List<DestProbInfo> destProbList;
}

[System.Serializable]
public struct DestProbInfo
{
    public Node destination;
    public float probabillity;
}

public class Node : MonoBehaviour
{
    public List<Citizen> citizenList;
    public List<DestinationInfo> possibleDestinationList;
    [SerializeField] float produceCitizenProb = 0.02f;

    [SerializeField] GameObject go;

    int currentHour;

    public IEnumerator ProduceCitizen()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            float rnd = Random.Range(0, 1.00f);
            if (rnd < produceCitizenProb)
            {
                GameObject citizenObj = Instantiate(go);
                Citizen citizen = citizenObj.GetComponent<Citizen>();
                citizen.destinationNode = GetDestinationNode();
            }

        }
    }

    private void Start()
    {
        StartCoroutine(ProduceCitizen());
    }

    public DestinationInfo GetDestinationInfo()
    {
        return possibleDestinationList.Find(x=>x.hourStart <= currentHour && x.hourEnd > currentHour); 
    }

    public Node GetDestinationNode()
    {
        float rnd = Random.Range(0, 1.00f);
        for (int i=0;i<GetDestinationInfo().destProbList.Count; i++)
        {
            DestProbInfo destProbInfo = GetDestinationInfo().destProbList[i];
            if (destProbInfo.probabillity > rnd)
            {
                return destProbInfo.destination;
            }
            else
            {
                rnd -= destProbInfo.probabillity;
            }
        }    
        return null;
    }
}
