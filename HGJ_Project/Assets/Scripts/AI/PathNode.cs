using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public List<PathNode> nextNodes = new List<PathNode>();
    public Transform targetNode;

    // Start is called before the first frame update
    void Start()
    {
        if(targetNode)
        {
            Transform bestNode = BestNode(targetNode);
            Debug.Log(bestNode.gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform BestNode(Transform targetNode)
    {
        Transform bestNode = null;
        List<Transform> visitedNodes = new List<Transform>();

        ClosestNode(targetNode, ref bestNode, 0, visitedNodes);

        return bestNode;
    }

    public float ClosestNode(Transform targetNode, ref Transform nextNode, float currDist, List<Transform> visitedNodes)
    {
        if(visitedNodes.Contains(transform))
            return -1;

        List<Transform> currentVisited = new List<Transform>();
        foreach (Transform t in visitedNodes) currentVisited.Add(t);
        currentVisited.Add(transform);

        if (transform == targetNode)
        {
            nextNode = transform;
            return 0;
        }

        float bestDist = -1;

        foreach(PathNode node in nextNodes)
        {

            Transform tempTransform = null;
            float tempDist = currDist + Vector3.Distance(node.transform.position, transform.position);
            float secondTempDist = node.ClosestNode(targetNode, ref tempTransform, tempDist, currentVisited);

            if (secondTempDist < 0) continue;
            tempDist += secondTempDist;

            if (bestDist < 0 || tempDist < bestDist)
            {
                bestDist = tempDist;
                nextNode = node.transform;
            }
        }

        return bestDist;
        
    }
}
