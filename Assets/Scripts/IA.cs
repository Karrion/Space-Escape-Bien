using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

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
    private CoverManagerMal coverManager;
    private SpriteRenderer interrogacion;
    private SpriteRenderer exclamacion;
    private bool escuchaAlgo = false;
    private Quaternion rotacionInicial;
    private int zona = 1;
    private int zonaAnterior;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        sphere = transform.GetChild(0).GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        interrogacion = transform.GetChild(2).GetComponent<SpriteRenderer>();
        exclamacion = transform.GetChild(1).GetComponent<SpriteRenderer>();
        currentPatrol = transform.position;
        rotacionInicial = transform.rotation;

       
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
        anim.SetBool("Caminar", false);
        yield return new WaitForSeconds(5);
        interrogacion.enabled = false;
        agent.destination = currentPatrol;
        escuchaAlgo = false;
        agent.Resume();
    }


    void Update()
    {
        zonaAnterior = GameController.zonaAnterior;
        zona = GameController.zona;
        

        // Debug.Log("Modo: " + mode);
        //Debug.Log(agent.remainingDistance);
        switch (mode){
            case Mode.Patrol:
                Patrol();
                if (escuchaAlgo)
                    calcularDistanciaEscuchado();
                break;
            case Mode.Alert:
                if (interrogacion.enabled == true)
                    interrogacion.enabled = false;
                exclamacion.enabled = true;
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
                agent.Stop();
                anim.SetTrigger("Hit");
                StartCoroutine("GetHit");
                Alcanzado();
                break;
            default:
                break;
        }
    }

    private IEnumerator GetHit()
    {
        yield return new WaitForSeconds(0.8f);
    }

    private void Shooting()
    {
        agent.SetDestination(player.transform.position);
        if (agent.remainingDistance <= 10f)
        {
            agent.Stop();
            RotateTowards(player.transform);
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
            if (agent.remainingDistance <= 10f)
            {
                mode = Mode.Shooting;
            }
            if (PlayerMovement.getMode() != PlayerMovement.Mode.Aiming)
                agent.destination = player.transform.position;
           /* if(PlayerMovement.getMode() == PlayerMovement.Mode.Aiming)
            {
                Transform Objetivo = CoverManagerMal.BuscarMasCercana(transform, (int) sphere.radius);
                if (Objetivo != null)
                {
                    runToCover = true;
                    agent.destination = Objetivo.position;
                }
            }*/
        }
        else
        {
            //agent.destination = player.transform.position;
            alertTime += Time.deltaTime;
            if(alertTime >= 4f && alertTime < 6f)
            {
                switch (zona)
                {
                    case 1:
                        Buscar(1);
                        break;
                    case 2:
                        Buscar(2);
                        break;
                    case 3:
                        Buscar(3);
                        break;
                    default:
                        break;
                }
            }
            /*if(alertTime >= 10.0f)
            {
                alertTime = 0;
                exclamacion.enabled = false;
                mode = Mode.Patrol;
                agent.destination = currentPatrol;
            }*/
        }
       
    }

    private void Patrol()
    {
        if (points.Length != 0)
        {
            anim.SetBool("Caminar", true);
        }
        else
        {
            if(!escuchaAlgo)
                anim.SetBool("Caminar", false);
            anim.SetBool("Correr", false);
            anim.SetBool("Disparar", false);
            if(agent.remainingDistance > 1.5f && !escuchaAlgo)
            {
                anim.SetBool("Caminar", true);

            }
            else
            {
                if (!escuchaAlgo)
                {
                    anim.SetBool("Caminar", false);
                    transform.rotation = rotacionInicial;
                    agent.Stop();
                }
                   
                
            }

        }


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
        anim.SetBool("Caminar", true);
        agent.Resume();
        interrogacion.enabled = true;
        if(points.Length != 0) currentPatrol = agent.destination;
        agent.destination = player.transform.position;
        escuchaAlgo = true;
        /*if (transform.position == agent.destination)
        {
       
            StartCoroutine("tiempoEspera");
         
        }*/
      
    }

    void Alcanzado()
    {
       
      /* if (coverManager.BuscarMasCercana(transform, sphere.radius) != null)
       {
            Transform Objetivo = coverManager.BuscarMasCercana(transform, sphere.radius);
            Debug.Log("Corro p'allá");
            runToCover = true;
            agent.destination = Objetivo.position;
            anim.SetBool("Disparar", false);
            anim.SetBool("Correr", true);
            agent.Resume();*/
            if (agent.remainingDistance <= 1f)
            {
                agent.Stop();
                anim.SetBool("Disparar", true);
                anim.SetBool("Correr", false);
                mode = Mode.Shooting;
                return;
            }
     //   }
        agent.SetDestination(player.transform.position);
        agent.Resume();
        mode = Mode.Alert;
    }

    public void getHit()
    {
        mode = Mode.Hit;
        anim.SetBool("Caminar", false);
        anim.SetBool("Correr", false);
        anim.SetBool("Disparar", false);
        Debug.Log("Au");
    }

    public void calcularDistanciaEscuchado()
    {       
        if (agent.remainingDistance <= 0.6)
        {
            StartCoroutine("tiempoEspera");
        }       
    }
    void Buscar(int zonaBusqueda)
    {
        Debug.Log(zonaBusqueda);

        GameObject nodoInicial = null;
        GameObject[] lista = new GameObject[0];
        GameObject nodoDestino;
        List<GameObject> listaOpen = new List<GameObject>();
        List<GameObject> listaClosed = new List<GameObject>();
        
        int contador = 0;
        float costeTotal = 0;
        Debug.Log(zonaAnterior);
        switch (zonaBusqueda)
        {
            case 1:
                lista = GameObject.FindGameObjectWithTag("Zona1").GetComponentsInChildren<GameObject>();
                nodoInicial = lista[10];
                break;
            case 2:
                lista = GameObject.FindGameObjectWithTag("Zona2").GetComponentsInChildren<GameObject>();
                if(zonaAnterior == 1)
                    nodoInicial = lista[8];
                else if(zonaAnterior == 3)
                    nodoInicial = lista[18];
                break;
            case 3:
                lista = GameObject.FindGameObjectWithTag("Zona3").GetComponentsInChildren<GameObject>();
                nodoInicial = lista[0];
                break;
        }
        Debug.Log(nodoInicial);
        if(nodoInicial == null)
            Debug.Log("no..");
        listaOpen.Add(nodoInicial);
        contador++;
        nodoDestino  = lista[UnityEngine.Random.Range(0, lista.Length)];
       
       
        while(contador != 0)
        {
            if (nodoInicial == nodoDestino)
                Debug.Log("ya he entrado");
            else
            {
                listaOpen.Remove(nodoInicial);
                contador--;
                listaClosed.Add(nodoInicial);
                int indice = 0;
                foreach (GameObject vecino in nodoInicial.GetComponent<WaypointBehaviour>().neighbours)
                {
                    if (!listaClosed.Contains(vecino))
                    {
                        costeTotal = costeTotal + nodoInicial.GetComponent<WaypointBehaviour>().values[indice];
                        if (!listaOpen.Contains(vecino))
                        {
                            listaOpen.Add(vecino);
                            contador++;
                        }
                        else if (costeTotal < nodoInicial.GetComponent<WaypointBehaviour>().values[indice])
                        {
                            agent.destination = nodoInicial.transform.position;
                            nodoInicial = vecino;
                            nodoInicial.GetComponent<WaypointBehaviour>().values[indice] = costeTotal;
                        }

                    }
                             
                    
                }

            }
        }

    }
}