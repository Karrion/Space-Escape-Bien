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

    private GameObject formacion11;
    private GameObject formacion12;

    public List<GameObject> listaNodos = new List<GameObject>();
    GameObject nodoDestino = null;



    // Use this for initialization
    void Start () {
        guardias = GameObject.FindGameObjectsWithTag("Enemy");
        formacion11 = GameObject.FindGameObjectWithTag("Formacion11");
        formacion12 = GameObject.FindGameObjectWithTag("Formacion12");
    }

   
	
	// Update is called once per frame
	void Update () {
       
    }

    public void EnemigoEnSala(GameObject guardia)
    {
        int nodos = 1;
        foreach (GameObject enemigo in guardias)
        {
            if (enemigo != guardia)
            {
                if (enemigo.GetComponent<IA>().zonaEnemigo == guardia.GetComponent<IA>().zonaEnemigo) {
                    switch(nodos){
                        case 1:
                            enemigo.GetComponent<NavMeshAgent>().destination = formacion11.transform.position;
                            enemigo.GetComponent<IA>().mode = IA.Mode.Puerta;
                            nodos++;
                            break;
                        case 2:
                            enemigo.GetComponent<NavMeshAgent>().destination = formacion12.transform.position;
                            enemigo.GetComponent<IA>().mode = IA.Mode.Puerta;
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
                if (enemigo.GetComponent<IA>().zonaEnemigo == guardia.GetComponent<IA>().zonaEnemigo)
                {
                    enemigo.GetComponent<IA>().switchGenerarListas();
                    enemigo.GetComponent<IA>().mode = IA.Mode.Search;
                }
            }
        }
        if (guardia.GetComponent<IA>().zonaPersonaje == 1)
        {
            taponador2.GetComponent<NavMeshAgent>().destination = taponZona1.transform.position;
            taponador2.GetComponent<IA>().mode = IA.Mode.Tapon;
        }
        else if (guardia.GetComponent<IA>().zonaPersonaje == 2)
        {
            taponador3.GetComponent<NavMeshAgent>().destination = taponZona2.transform.position;
            taponador1.GetComponent<NavMeshAgent>().destination = taponZona1.transform.position;
            taponador3.GetComponent<IA>().mode = IA.Mode.Tapon;
            taponador1.GetComponent<IA>().mode = IA.Mode.Tapon;
        }
        else if (guardia.GetComponent<IA>().zonaPersonaje == 3)
        {
            taponador2.GetComponent<NavMeshAgent>().destination = taponZona3.transform.position;
            taponador2.GetComponent<IA>().mode = IA.Mode.Tapon;
        }
    }

    public void terminarBusqueda()
    {
        taponador1.GetComponent<IA>().mode = IA.Mode.Patrol;
        taponador2.GetComponent<IA>().mode = IA.Mode.Patrol;
        taponador3.GetComponent<IA>().mode = IA.Mode.Patrol;
    }
}
