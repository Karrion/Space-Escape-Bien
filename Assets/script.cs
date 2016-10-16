using UnityEngine;
using System.Collections;


public class script : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private float visibilityDistance = 10f;
    private bool modoAlerta = false;
    public GameObject player;
    private int fieldOfViewDegrees = 90;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 rayDirection = player.transform.position - transform.position;
        if (!modoAlerta)
        {
            if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees * 0.5f)
            {
                if (Physics.Raycast(transform.position, rayDirection, out hit, visibilityDistance))
                {
                    Debug.DrawRay(transform.position, hit.transform.position);
                    modoAlerta = true;
                    Debug.Log("Te veo");
                    return (hit.transform.CompareTag("Player"));
                }
            }
        }
        return false;
    }


    void Update()
    {
        if (agent.remainingDistance < 0.5f && !modoAlerta)
            GotoNextPoint();

        if (CanSeePlayer())
        {
            agent.autoBraking = true;
            agent.destination = player.transform.position;
        }

    }
}