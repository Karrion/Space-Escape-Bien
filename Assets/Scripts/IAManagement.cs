using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

public class IAManagement : MonoBehaviour {
    Transform objectivePosition;
    Transform playerPosition;
    private int zona = 1;
    GameObject[] guardias;
    List<GameObject> tiosTapon = new List<GameObject>();
    public GameObject taponador1;
    public GameObject taponador2;
    public GameObject taponador3;
    public GameObject taponZona1;
    public GameObject taponZona2;
    public GameObject taponZona3;

    public GameObject esconditeZona1;
    public GameObject esconditeZona2;
    public GameObject esconditeZona3;

    public GameObject cobertura1;
    public GameObject cobertura2;
    public GameObject cobertura3;
    public bool estoyCubierto;




    public List<GameObject> listaNodosSalas = new List<GameObject>();
    GameObject nodoDestino = null;
    public bool hayMasGenteApuntando = false;




    // Use this for initialization
    void Start () {
        guardias = GameObject.FindGameObjectsWithTag("Enemy");
        
    }

    public void EnemigoEnSala(GameObject guardia, GameObject nodo1, GameObject nodo2)
    {
        int nodos = 1;
        foreach (GameObject enemigo in guardias)
        {
            if (enemigo != guardia)
            {
                if (enemigo.GetComponent<IA>().zonaEnemigo == guardia.GetComponent<IA>().zonaEnemigo) {
                    switch(nodos){
                        case 1:
                            enemigo.GetComponent<NavMeshAgent>().destination = nodo1.transform.position;
                            nodos++;
                            break;
                        case 2:
                            enemigo.GetComponent<NavMeshAgent>().destination = nodo2.transform.position;
                            nodos++;
                            break;
                        default:
                            break;
                    }
                    if (nodos > 2) return;
                }
            }
        }
    }

    public void EnemigoVisto(GameObject guardia)
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        foreach (GameObject enemigo in guardias)
        {
            if (enemigo != guardia)
            {
                if (enemigo.GetComponent<IA>().zonaEnemigo == guardia.GetComponent<IA>().zonaEnemigo && !enemigo.GetComponent<IA>().escondiendose)
                {
                    enemigo.GetComponent<IA>().switchGenerarListas();
                    enemigo.GetComponent<IA>().mode = IA.Mode.Search;
                }
            }
        }
        if (guardia.GetComponent<IA>().zonaPersonaje == 1 && taponador2.GetComponent<IA>().mode != IA.Mode.Alert)
        {
            taponador2.GetComponent<NavMeshAgent>().destination = taponZona1.transform.position;
            taponador2.GetComponent<IA>().taponando = true;
            taponador2.GetComponent<IA>().mode = IA.Mode.Patrol;



        }
        else if (guardia.GetComponent<IA>().zonaPersonaje == 2)
        {
            if (taponador3.GetComponent<IA>().mode != IA.Mode.Alert)
            {
                taponador3.GetComponent<NavMeshAgent>().destination = taponZona2.transform.position;
                taponador3.GetComponent<IA>().taponando = true;
                taponador3.GetComponent<IA>().mode = IA.Mode.Patrol;

            }
            if (taponador1.GetComponent<IA>().mode != IA.Mode.Alert)
            {
                taponador1.GetComponent<NavMeshAgent>().destination = taponZona1.transform.position;
                taponador1.GetComponent<IA>().taponando = true;
                taponador1.GetComponent<IA>().mode = IA.Mode.Patrol;
            }
        }
        else if (guardia.GetComponent<IA>().zonaPersonaje == 3 && taponador3.GetComponent<IA>().mode != IA.Mode.Alert)
        {
            taponador2.GetComponent<NavMeshAgent>().destination = taponZona3.transform.position;
            

            taponador2.GetComponent<IA>().taponando = true;

            taponador2.GetComponent<IA>().mode = IA.Mode.Patrol;
        }
    }

    public void terminarBusqueda()
    {
        taponador1.GetComponent<IA>().mode = IA.Mode.Patrol;
        taponador2.GetComponent<IA>().mode = IA.Mode.Patrol;
        taponador3.GetComponent<IA>().mode = IA.Mode.Patrol;
    }

    public void huir(GameObject guardia) {
        foreach(GameObject enemigo in guardias)
        {
            if(enemigo.GetComponent<IA>().mode == IA.Mode.Shooting)
            {
                hayMasGenteApuntando = true;
                break;
            }
            hayMasGenteApuntando = false;
        }
        if (!hayMasGenteApuntando)
        {
            switch (guardia.GetComponent<IA>().zonaAnterior)
            {
                case 1:
                    guardia.GetComponent<NavMeshAgent>().destination = esconditeZona3.transform.position;
                    break;
                case 2:
                    guardia.GetComponent<NavMeshAgent>().destination = esconditeZona2.transform.position;
                    break;
                case 3:
                    guardia.GetComponent<NavMeshAgent>().destination = esconditeZona1.transform.position;
                    break;
            }
            if (guardia.GetComponent<NavMeshAgent>().remainingDistance < 0.3f)
            {
                StartCoroutine("esperaEscondido", guardia);

            }
        }
    }

    private IEnumerator esperaEscondido(GameObject guardia)
    {
        guardia.GetComponent<NavMeshAgent>().Stop();
        yield return new WaitForSeconds(5f);
        guardia.GetComponent<NavMeshAgent>().Resume();
        guardia.GetComponent<IA>().escondiendose = false;
        guardia.GetComponent<IA>().mode = IA.Mode.Patrol;
    }

    public void Coberturas(GameObject guardia)
    {
        if (guardia.transform.GetChild(0).GetComponent<Deteccion>().irACobertura) {
            estoyCubierto = true;
            Debug.Log("de camino a la cobertura");
           // guardia.GetComponent<IA>().mode = IA.Mode.Hit;
            switch (guardia.GetComponent<IA>().zonaEnemigo)
            {
                case 1:
                    guardia.GetComponent<NavMeshAgent>().destination = cobertura1.transform.position;
                    break;
                case 2:
                    guardia.GetComponent<NavMeshAgent>().destination = cobertura2.transform.position;
                    break;
                case 3:
                    guardia.GetComponent<NavMeshAgent>().destination = cobertura3.transform.position;
                    break;
            }
            
            /*if (guardia.GetComponent<NavMeshAgent>().remainingDistance < 1f) {
                Debug.Log("me paro");
                guardia.GetComponent<NavMeshAgent>().Stop();
                estoyCubierto = false;
            } else {
              //  estoyCubierto = false;
            };*/
        }
    }
}
