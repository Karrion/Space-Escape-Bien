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
    bool delReves = false;
    public static bool runToCover = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.autoBraking = false;
        sphere = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public bool getRunToCover()
    {
        return runToCover;
    }

    void GotoNextPoint()
    {

        if (points.Length == 0)
            return;

        if (!delReves)
        {
            agent.destination = points[destPoint].position;
            destPoint++;
            if (destPoint == points.Length - 1) delReves = true;
        }
        else
        {
            agent.destination = points[destPoint].position;
            destPoint--;
            if (destPoint == 0) delReves = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            inSight = false;
            RaycastHit hit;
            Vector3 rayDirection = player.transform.position - transform.position + transform.up;
            if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees * 0.5f)
            {
                if (Physics.Raycast(transform.position + transform.up, rayDirection.normalized, out hit, sphere.radius))
                {
                    Debug.DrawLine(transform.position + transform.up, hit.point);
                    if (hit.transform.CompareTag("Player"))
                    {
                        if (mode != Mode.Shooting)
                        {
                            currentPatrol = agent.destination;
                            mode = Mode.Alert;
                            inSight = true;
                            agent.autoBraking = true;
                        }
                    }
                    else {
                        if (mode == Mode.Shooting)
                          mode = Mode.Alert;
                            
                    }
                }
                else
                {
                    inSight = false;
                }          
            }
            else if (PlayerMovement.Running && mode == Mode.Patrol)
            {
                /*currentPatrol = agent.destination;
                mode = Mode.Alert;
                inSight = true;
                agent.autoBraking = true;*/
                escuchado();
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
            if (PlayerMovement.Running && mode == Mode.Patrol)
            {
                /* currentPatrol = agent.destination;
                 agent.destination = player.transform.position;
                 if(agent.remainingDistance >= 0)
                 {
                     StartCoroutine("tiempoEspera");

                 }*/
                escuchado();
            }
            

    }
    IEnumerator tiempoEspera()
    {
        agent.Stop();
        yield return new WaitForSeconds(5);
        agent.destination = currentPatrol;
        agent.Resume();
    }

    void Update()
    {
        //Debug.Log(mode);
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
        
        if (agent.remainingDistance <= 8f)
        {         
            agent.Stop();
            if (PlayerMovement.getMode() != PlayerMovement.Mode.Aiming)
                agent.destination = player.transform.position;
            RotateTowards(player.transform);
            if (PlayerMovement.getMode() == PlayerMovement.Mode.Aiming)
            {
                Transform Objetivo = CoverManagerMal.BuscarMasCercana(transform, (int) sphere.radius);
                if (Objetivo != null)
                {
                    Debug.Log("Corro p'allá");
                    runToCover = true;
                    agent.destination = Objetivo.position;
                    anim.SetBool("Disparar", false);
                    anim.SetBool("Correr", true);
                    agent.Resume();
                    if (agent.remainingDistance <= 1f)
                    {
                        agent.Stop();
                        anim.SetBool("Disparar", true);
                        anim.SetBool("Correr", false);
                    }
                }
            }
        }
        else
        {      
            agent.Resume();
            mode = Mode.Alert;           
        }
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
    }

    private void Alert()
    {
        if(inSight == true)
        {
            if (PlayerMovement.getMode() != PlayerMovement.Mode.Aiming)
                agent.destination = player.transform.position;
            if(PlayerMovement.getMode() == PlayerMovement.Mode.Aiming)
            {
                Transform Objetivo = CoverManagerMal.BuscarMasCercana(transform, (int) sphere.radius);
                if (Objetivo != null)
                {
                    runToCover = true;
                    agent.destination = Objetivo.position;
                }
            }
        }
        else
        {
            agent.destination = player.transform.position;
            alertTime += Time.deltaTime;
            if(alertTime >= 10.0f)
            {
                alertTime = 0;
                mode = Mode.Patrol;
                agent.destination = currentPatrol;
            }
        }
        if (agent.remainingDistance <= 8f && inSight)
        {
            mode = Mode.Shooting;
        }    
    }

    private void Patrol()
    {
        if (points.Length != 0) anim.SetBool("Caminar", true);
        else anim.SetBool("Caminar", false);
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
    private void escuchado()
    {
        if (PlayerMovement.Running && mode != Mode.Shooting)
        {
            currentPatrol = agent.destination;
            agent.destination = player.transform.position;
            if (agent.remainingDistance >= 0)
            {
                StartCoroutine("tiempoEspera");

            }
        }
    }
}