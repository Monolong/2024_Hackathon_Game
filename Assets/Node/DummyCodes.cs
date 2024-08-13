using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dummy
{
    public class Citizen : MonoBehaviour
    {
        public Station destinationStation;
        public Node destinationNode;

        Vector3 dirVector;
        float moveSpeed = 1f;

        private void Update()
        {
            dirVector = destinationNode.transform.position - transform.position;
            transform.position += dirVector * moveSpeed * Time.deltaTime;  
        }
    }
}

