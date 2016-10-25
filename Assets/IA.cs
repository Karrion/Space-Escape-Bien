using UnityEngine;
using System.Collections;
using System;

public class IA : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    public GameObject player;
    private int fieldOfViewDegrees = 110;
    enum Mode { Alert, Patrol, Shooting };
    Mode mode = Mode.Patrol;
    private float alertTime = 0.0f;
    private Vector3 currentPatrol;
    private bool inSight;
    private SphereCollider sphere;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.autoBraking = false;
        sphere = GetComponent<SphereCollider>();
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
            inSight = false;
            RaycastHit hit;
            Vector3 rayDirection = player.transform.position - transform.position;
            if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees * 0.5f)
            {
                if (Physics.Raycast(transform.position + transform.up, rayDirection.normalized, out hit, sphere.radius))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        if (mode != Mode.Shooting)
                        {
                            mode = Mode.Alert;
                            inSight = true;
                            currentPatrol = agent.destination;
                            agent.autoBraking = true;
                        }
                    }
                }
                else
                {
                    inSight = false;
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
            case Mode.Shooting:
                Shooting();
                break;
            default:
                break;
        }

    }

    private void Shooting()
    {
        if (agent.remainingDistance <= 10f)
        {
            agent.Stop();
            agent.destination = player.transform.position;
        }
        else
        {
            
            Debug.Log("Marchamos en loca persecución");
            agent.Resume();
            mode = Mode.Alert;
            
        }
    }

    private void Alert()
    {
        if(inSight == true)
        {
            agent.destination = player.transform.position;
        }
        else
        {
            alertTime += Time.deltaTime;
            if(alertTime >= 10.0f)
            {
                alertTime = 0;
                mode = Mode.Patrol;
                agent.destination = currentPatrol;
            }
        }
        if(agent.remainingDistance <= 2.5f && inSight)
        {
            mode = Mode.Shooting;
        }
    }

    private void Patrol()
    {
       if (agent.autoBraking == true) agent.autoBraking = false;
       if (agent.remainingDistance < 1)
        {
            GotoNextPoint();
        }
    }
}