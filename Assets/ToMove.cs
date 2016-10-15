// MoveTo.cs
using UnityEngine;
using System.Collections;

public class ToMove : MonoBehaviour
{

    public Transform goal;
    public Transform goal2;
    NavMeshAgent agent;
    bool Waypointreached = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }

    void Update()
    {
        Debug.Log(agent.remainingDistance);
        if (Waypointreached == false)
        {
            agent.destination = goal.position;
            if (agent.remainingDistance < 0.1)
            {
                Waypointreached = true;
            }
        }
        else
        {
            agent.destination = goal2.position;
        }
    }
}