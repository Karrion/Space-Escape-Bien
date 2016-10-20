using UnityEngine;
using System.Collections;
using System;

public class IA : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private float visibilityDistance = 10f;
    public GameObject player;
    private int fieldOfViewDegrees = 90;
    private LineRenderer line;
    enum Mode { Alert, Patrol };
    Mode mode = Mode.Patrol;
    private float alertTime = 0.0f;
    private Vector3 currentPatrol;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        agent.autoBraking = false;
        GotoNextPoint();
    }


    void GotoNextPoint()
    {

        if (points.Length == 0)
            return;

        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            RaycastHit hit;
            Vector3 rayDirection = player.transform.position - transform.position;
            if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees * 0.5f)
            {
                if (Physics.Raycast(transform.position, rayDirection, out hit, visibilityDistance))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        mode = Mode.Alert;
                        Debug.Log("Te veo");
                        currentPatrol = agent.destination;
                        line.SetPosition(0, transform.position);
                        line.SetPosition(1, hit.transform.position);
                        agent.destination = player.transform.position;
                    }
                }
                else
                {
                    Debug.Log("No veo");
                    mode = Mode.Patrol;
                }          
            }
        }
    }

    void Update()
    {
        switch (mode){
            case Mode.Patrol:
                Patrol();
                break;
            case Mode.Alert:
                Alert();
                break;
            default:
                break;
        }

    }

    private void Alert()
    {
        throw new NotImplementedException();
    }

    private void Patrol()
    {

       if (agent.remainingDistance < 1)
        {
            GotoNextPoint();
        }
    }
}