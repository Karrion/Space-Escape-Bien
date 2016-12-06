using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class IA : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private GameObject player;
    [HideInInspector] public int fieldOfViewDegrees = 110;
    public enum Mode { Alert, Patrol, Shooting, Hit, Search, Escuchado, Tapon};
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
    [HideInInspector]public int zonaPersonaje = 1;
    [HideInInspector]public int zonaAnterior;
    private bool empezarBusqueda = true;
    public List<GameObject> listaNodos = new List<GameObject>();
    GameObject nodoDestino = null;
    private int r;
    public bool escuchaBuscando = false;
    public bool disparar = true;
    public float ShootTimer = 0.0f;
    public int zonaEnemigo;
    private IAManagement iamanagement;

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
        iamanagement = GameObject.FindGameObjectWithTag("IaManagement").GetComponent<IAManagement>();
    }

    /*public int getZone()
    {
        if
    }*/

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
        zonaPersonaje = GameController.zona;
        Debug.Log(gameObject.name + ", " + mode);
        
        //Debug.Log(zona);
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
                anim.SetBool("Apuntar", false);
                Alert();
                break;
            case Mode.Shooting:
                anim.SetBool("Apuntar", true);
                anim.SetBool("Disparar", false);
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
            case Mode.Search:
                anim.SetBool("Correr", false);
                anim.SetBool("Caminar", true);
                anim.SetBool("Disparar", false);
                anim.SetBool("Apuntar", false);
                Search();
                break;
            case Mode.Tapon:
                taponando();

                break;
            default:
                break;
        }
    }

    private void taponando() {

        if (agent.remainingDistance <= 0.3f) {
            agent.Stop();
            agent.Resume();

            anim.SetBool("Correr", false);
            anim.SetBool("Caminar", false);
            anim.SetBool("Disparar", false);
            anim.SetBool("Apuntar", false);

        }
    }

    private IEnumerator GetHit()
    {
        yield return new WaitForSeconds(0.8f);
    }

    private void Shooting()
    {
        ShootTimer += Time.deltaTime;
        agent.SetDestination(player.transform.position);
        if (agent.remainingDistance <= 10f)
        {
            agent.Stop();
            RotateTowards(player.transform);
            Debug.Log(ShootTimer);
            
            if (disparar)
            {
                anim.SetBool("Apuntar", false);
                anim.SetBool("Disparar", true);
                ShootTimer = 0.0f;
            }
            else if(ShootTimer >= 8f)
            {
                SceneManager.LoadScene("CharacterThirdPerson");
            }
        }
        else
        {      
            agent.Resume();
            disparar = false;
            mode = Mode.Alert;           
        }
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
    }

    private void Alert()
    {
        escuchaAlgo = false;
        if(inSight == true)
        {
            empezarBusqueda = true;
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
           // agent.destination = player.transform.position;
            alertTime += Time.deltaTime;
            iamanagement.EnemigoVisto(gameObject);
            if (alertTime < 4f)
            {
                agent.destination = player.transform.position;
            }
            if (alertTime >= 4f && alertTime < 6f && empezarBusqueda)
            {
                switchGenerarListas();
                empezarBusqueda = false;
                agent.destination = nodoDestino.transform.position;
                alertTime = 0;
                mode = Mode.Search;
            }
        }
    }

    public void switchGenerarListas() {
        switch (zonaPersonaje)
        {
            case 1:
                generarLista(1);
                break;
            case 2:
                generarLista(2);
                break;
            case 3:
                generarLista(3);
                break;
            default:
                Debug.Log("Tengo la zona mal puesta");
                break;
        }
    }

    private void Search()
    {
        if (!escuchaBuscando)
        {
            if (agent.remainingDistance <= 0.3f && listaNodos.Count > 0)
            {
                Debug.Log("Llego a mi nodo");
                agent.Stop();
                nuevoNodo(nodoDestino);
                StartCoroutine("esperaBusqueda");

                if (listaNodos.Count <= 0)
                {
                    exclamacion.enabled = false;
                    mode = Mode.Patrol;
                    agent.destination = currentPatrol;
                }

                agent.Resume();
            }
        }
        else
        {
            if (agent.remainingDistance <= 0.3f)
            {
                agent.Stop();
                StartCoroutine("tiempoEspera");
                agent.destination = nodoDestino.transform.position;
                agent.Resume();
                escuchaBuscando = false;
            }
        }
    }
  

    private void nuevoNodo(GameObject nodoDestino)
    {
        listaNodos.RemoveAt(r);
        if (listaNodos.Count != 0)
        {
            r = UnityEngine.Random.Range(0, listaNodos.Count);
            //Debug.Log("Random: " + r + " Tamaño: " + listaNodos.Count);
            listaNodos.Remove(nodoDestino);
            nodoDestino = listaNodos[r];
            agent.destination = nodoDestino.transform.position;
        }
        Debug.Log(listaNodos.Count);
    }

    private IEnumerator esperaBusqueda()
    {
        yield return new WaitForSeconds(2f);
    }

    private void Patrol()
    {
        if (points.Length != 0 || agent.remainingDistance > 0.1f)
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
        if (transform.position == agent.destination)
        {
            StartCoroutine("tiempoEspera");
        }
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

    void generarLista(int zonaBusqueda)
    {
        GameObject[] vectorNodos = new GameObject[100];
        listaNodos = new List<GameObject>();

        switch (zonaBusqueda)
        {
            case 1:
                vectorNodos = GameObject.FindGameObjectsWithTag("VertexZona1");
                Debug.Log("Genero lista de nodos de zona 1");
                nodoDestino = vectorNodos[10];
                break;
            case 2:
                vectorNodos = GameObject.FindGameObjectsWithTag("VertexZona2");
                Debug.Log("Genero lista de nodos de zona 2");
                if (zonaAnterior == 1)
                    nodoDestino = vectorNodos[8];
                else if (zonaAnterior == 3)
                    nodoDestino = vectorNodos[18];
                break;
            case 3:
                vectorNodos = GameObject.FindGameObjectsWithTag("VertexZona3");
                Debug.Log("Genero lista de nodos de zona 3");
                nodoDestino = vectorNodos[0];
                break;
        }
        listaNodos = vectorNodos.ToList();
        r = listaNodos.IndexOf(nodoDestino);
    }
}