using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBasic : MonoBehaviour
{
    // AI-Controller
    AIController aiCon;

    // State Container
    delegate void RunState();
    Dictionary<string, RunState> stateLibrary = new Dictionary<string, RunState>();
    string currentState;

    // Enemy Info
    List<GameObject> enemyList = new List<GameObject>();
    GameObject targetEnemy;

    // Environment Info
    public List<PathNode> pathNodes = new List<PathNode>();

    // Test Variables
    public PathNode targetNode;
    PathNode nextNode;
    PathNode baseNode;
    float lerpalpha;
    float targetRotation;
    Vector3 targetPosition;
    float chargeLifeTime;
    float restTimer;
    bool targetingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        InitializeBasicAI();
    }

    // Update is called once per frame
    void Update()
    {
        if (stateLibrary.ContainsKey(currentState))
            stateLibrary[currentState]();
    }

    void InitializeBasicAI()
    {
        // Initialize AI Controller
        aiCon = GetComponent<AIController>();
        currentState = "InitialState";
        stateLibrary.Add("InitialState", InitialState);
        stateLibrary.Add("ChangeTarget", ChangeTarget);
        stateLibrary.Add("SetRotation", SetRotation);
        stateLibrary.Add("RotateState", RotateState);
        //stateLibrary.Add("SearchingState", SearchingState);
        stateLibrary.Add("ChargeState", ChargeState);
        stateLibrary.Add("RestState", RestState);

        // Initialize enemy list
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player != gameObject)
                enemyList.Add(player);
        }
    }


    void InitialState()
    {
        currentState = "ChangeTarget";
        targetEnemy = enemyList[0];
        ResetNextNode();
    }

    void ChangeTarget()
    {
        currentState = "SetRotation";

        foreach (GameObject enemy in enemyList)
        {
            if (!enemy.activeSelf)
                continue;
            if (!targetEnemy)
                targetEnemy = enemy;
        }
        if (!targetEnemy)
            return;
        ResetNextNode();

        AIController enemyAI = targetEnemy.GetComponent<AIController>();
        PlayerMovement enemyPlayer = targetEnemy.GetComponent<PlayerMovement>();

        if(enemyAI)
        {
            if(enemyAI.isGrounded || enemyAI.transform.position.y > transform.position.y)
            {
                if (Vector3.Distance(transform.position, enemyAI.transform.position) <= 10)
                    return;
            }
        }
        else
        {
            if (enemyPlayer.isGrounded || enemyPlayer.transform.position.y > transform.position.y)
            {
                if (Vector3.Distance(transform.position, enemyPlayer.transform.position) <= 10)
                    return;
            }
        }

        foreach(GameObject enemy in enemyList)
        {
            AIController newAI = enemy.GetComponent<AIController>();
            PlayerMovement newPlayer = enemy.GetComponent<PlayerMovement>();

            if (enemy.transform.position.y < 8)
                continue;

            if(newAI)
            {
                if (newAI.isGrounded || newAI.transform.position.y > transform.position.y)
                {
                    if (Vector3.Distance(transform.position, newAI.transform.position) <= 10)
                    {
                        targetEnemy = enemy;
                        return;
                    }
                }
            }
            else
            {
                if (newPlayer.isGrounded || newPlayer.transform.position.y > transform.position.y)
                {
                    if (Vector3.Distance(transform.position, newPlayer.transform.position) <= 10)
                    {
                        targetEnemy = enemy;
                        return;
                    }
                }
            }
        }

    }

    void SetRotation()
    {
        targetPosition = Vector3.zero;

        if (Vector3.Distance(targetEnemy.transform.position, transform.position) < 10)
        {
            targetPosition = targetEnemy.transform.position;
            targetingPlayer = true;
        }
        
        else
        {
            targetPosition = nextNode.transform.position;
            targetingPlayer = false;
        }
        
        Vector3 originalRot = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.LookAt(targetPosition);
        targetRotation = transform.eulerAngles.y;
        transform.eulerAngles = originalRot;

        if (targetingPlayer)
            targetRotation += Random.Range(-10, 10);

        currentState = "RotateState";
    }

    void RotateState()
    {
        if (transform.eulerAngles.y - targetRotation > 10 || transform.eulerAngles.y - targetRotation < -10)
        {
            aiCon.Steer(targetRotation);
        }
        else
        {
            currentState = "ChargeState";
            aiCon.releasedDirection = transform.forward;
            float chargePower = Vector3.Distance(targetPosition, transform.position) / 2.0f * 0.1f;
            //Debug.Log( gameObject.name + ": " + chargePower);
            if (chargePower > 0.75f) chargePower = 0.75f;
            if (targetingPlayer) chargePower = 0.25f;
            aiCon.Charge(chargePower);
            //aiCon.Charge(0.1f);
        }
    }

    void ChargeState()
    {
        if (aiCon.chargeTime <= 0)
        {
            currentState = "RestState";
            restTimer = 1.0f;
        }
        else
            transform.LookAt(targetPosition);
    }

    void RestState()
    {
        if (restTimer <= 0)
            currentState = "ChangeTarget";
        else
            restTimer -= Time.deltaTime;
    }

    void ResetNextNode()
    {
        baseNode = null;
        foreach (PathNode node in pathNodes)
        {
            if (Vector3.Distance(node.transform.position, transform.position) < 5)
                baseNode = node;
        }

        if(baseNode)
        {
            PathNode targetNode = pathNodes[0];
            foreach(PathNode node in pathNodes)
            {
                if (Vector3.Distance(targetNode.transform.position, targetEnemy.transform.position) >
                    Vector3.Distance(node.transform.position, targetEnemy.transform.position))
                    targetNode = node;
            }

            nextNode = baseNode.BestNode(targetNode.transform).GetComponent<PathNode>();
        }
        else
        {
            nextNode = pathNodes[0];
            foreach (PathNode node in pathNodes)
            {
                if (Vector3.Distance(nextNode.transform.position, transform.position) >
                    Vector3.Distance(node.transform.position, transform.position))
                    nextNode = node;
            }
        }

        //Debug.Log("Searching " + targetEnemy.name + " and " + nextNode.name);

    }


}
