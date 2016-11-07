using UnityEngine;
using System.Collections;
using System;

public class IA : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private GameObject player;
    private int fieldOfViewDegrees = 110;
    enum Mode { Alert, Patrol, Shooting };
    Mode mode = Mode.Patrol;
    private float alertTime = 0.0f;
    private Vector3 currentPatrol;
    private bool inSight;
    private SphereCollider sphere;
    private Animator anim;
    private Rigidbody rigidbody;
    public float rotationSpeed = 10f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.autoBraking = false;
        sphere = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
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
        //Debug.Log("Correr = " + anim.GetBool("Correr") + " Caminar = " + anim.GetBool("Caminar") + " Disparar = " + anim.GetBool("Disparar"));
        switch (mode){
            case Mode.Patrol:
                Patrol();
                break;
            case Mode.Alert:
                anim.SetBool("Correr", true);
                anim.SetBool("Caminar", false);
                anim.SetBool("Disparar", false);
                Alert();
                break;
            case Mode.Shooting:
                anim.SetBool("Disparar", true);
                anim.SetBool("Correr", false);
                anim.SetBool("Caminar", false);
                Shooting();
                break;
            default:
                break;
        }

    }

    private void Shooting()
    {
        
        if (agent.remainingDistance <= 20f)
        {
            agent.Stop();
            agent.destination = player.transform.position;
            RotateTowards(player.transform);
            
            
        }
        else
        {
            
            Debug.Log("Marchamos en loca persecución");
            agent.Resume();
            mode = Mode.Alert;
            
        }
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
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
        anim.SetBool("Caminar", true);
        anim.SetBool("Correr", false);
        anim.SetBool("Disparar", false);
        if (agent.autoBraking == true) agent.autoBraking = false;
       if (agent.remainingDistance < 1)
        {
            GotoNextPoint();
        }
    }


    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}