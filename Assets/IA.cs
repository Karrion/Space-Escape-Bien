using UnityEngine;
using System.Collections;
using System;

public class IA : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private GameObject player;
    [HideInInspector] public int fieldOfViewDegrees = 110;
    public enum Mode { Alert, Patrol, Shooting, Hit };
    public Mode mode = Mode.Patrol;
    private float alertTime = 0.0f;
    [HideInInspector] public Vector3 currentPatrol;
    [HideInInspector] public bool inSight;
    private GameObject sphereGameObject;
    private SphereCollider sphere;
    private Animator anim;
    private Rigidbody rigidbody;
    public float rotationSpeed = 10f;
    bool delReves = false;
    public static bool runToCover = false;
    private Mode previousMode;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.autoBraking = false;
        //sphereGameObject = GameObject.FindGameObjectWithTag("SphereEnemy");
        //sphere = sphereGameObject.GetComponent<SphereCollider>();
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


    IEnumerator tiempoEspera()
    {
        agent.Stop();
        yield return new WaitForSeconds(5);
        agent.destination = currentPatrol;
        agent.Resume();
    }


    void Update()
    {
        
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
            case Mode.Hit:
                anim.SetTrigger("Hit");
                agent.Stop();
                StartCoroutine("GetHit");

                break;
            default:
                break;
        }
    }

    private IEnumerator GetHit()
    {
        yield return new WaitForSeconds(0.9f);
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

    public void escuchado()
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

    public void getHit()
    {
        previousMode = mode;
        mode = Mode.Hit;
    }
}