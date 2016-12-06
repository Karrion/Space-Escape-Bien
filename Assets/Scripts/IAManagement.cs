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

    public List<GameObject> listaNodos = new List<GameObject>();
    GameObject nodoDestino = null;



    // Use this for initialization
    void Start () {
        guardias = GameObject.FindGameObjectsWithTag("Enemy");

       /* nodosZ1 = GameObject.FindGameObjectsWithTag("TaponarZona1").ToList<GameObject>();
        nodosZ2 = GameObject.FindGameObjectsWithTag("TaponarZona2").ToList<GameObject>();
        nodosZ3 = GameObject.FindGameObjectsWithTag("TaponarZona3").ToList<GameObject>();*/
    }

   /* void generarLista(GameObject guardia, GameObject enemigo) {
        GameObject[] vectorNodos = new GameObject[10];
        listaNodos = new List<GameObject>();
        switch (guardia.GetComponent<IA>().zonaEnemigo)
        {
            case 1:
                //if (enemigo.GetComponent<IA>().zonaEnemigo == 2 || enemigo.GetComponent<IA>().zonaEnemigo == 1)
                //{
                    vectorNodos = GameObject.FindGameObjectsWithTag("TaponarZona1");
                    Debug.Log("Genero lista de nodos para taponar de zona 1");
                    nodoDestino = vectorNodos[0];
                //}
                break;
            case 2:
                
                    vectorNodos = GameObject.FindGameObjectsWithTag("TaponarZona2");
                    Debug.Log("Genero lista de nodos para taponar de zona 2");
                    nodoDestino = vectorNodos[0];
                
                break;
            case 3:
               // if (enemigo.GetComponent<IA>().zonaEnemigo == 2 || enemigo.GetComponent<IA>().zonaEnemigo == 1)
                //{
                    vectorNodos = GameObject.FindGameObjectsWithTag("TaponarZona3");
                    Debug.Log("Genero lista de nodos para taponar de zona 3");
                    nodoDestino = vectorNodos[0];
                //}
                break;
        }
    }*/
	
	// Update is called once per frame
	void Update () {
       
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
