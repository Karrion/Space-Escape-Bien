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


    public List<GameObject> listaNodos = new List<GameObject>();
    GameObject nodoDestino = null;



    // Use this for initialization
    void Start () {
        guardias = GameObject.FindGameObjectsWithTag("Enemy");
	}

    void generarLista(GameObject enemigo) {
        GameObject[] vectorNodos = new GameObject[10];
        listaNodos = new List<GameObject>();
        switch (enemigo.GetComponent<IA>().zonaEnemigo)
        {
            case 1:
                vectorNodos = GameObject.FindGameObjectsWithTag("TaponarZona1");
                Debug.Log("Genero lista de nodos para taponar de zona 1");
                nodoDestino = vectorNodos[0];
                break;
            case 2:
                vectorNodos = GameObject.FindGameObjectsWithTag("TaponarZona2");
                Debug.Log("Genero lista de nodos para taponar de zona 2");
                nodoDestino = vectorNodos[0];
                break;
            case 3:
                vectorNodos = GameObject.FindGameObjectsWithTag("TaponarZona3");
                Debug.Log("Genero lista de nodos para taponar de zona 3");
                nodoDestino = vectorNodos[0];
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    public void EnemigoVisto(GameObject guardia)
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        foreach(GameObject enemigo in guardias)
        {
            if (enemigo != guardia)
            {
                if (enemigo.GetComponent<IA>().zonaEnemigo == guardia.GetComponent<IA>().zonaEnemigo) {
                    enemigo.GetComponent<IA>().switchGenerarListas();
                    enemigo.GetComponent<IA>().mode = IA.Mode.Search;
                }
                if (enemigo.GetComponent<IA>().zonaEnemigo != guardia.GetComponent<IA>().zonaEnemigo)
                {
                    enemigo.GetComponent<IA>().mode = IA.Mode.Tapon;

                    if (enemigo.GetComponent<IA>().zonaEnemigo == 1 && enemigo.GetComponent<IA>().zonaPersonaje == 2)
                    {
                        generarLista(enemigo);
                        enemigo.GetComponent<NavMeshAgent>().destination = nodoDestino.transform.position;
                        


                    }
                    else if (enemigo.GetComponent<IA>().zonaEnemigo == 3 && enemigo.GetComponent<IA>().zonaPersonaje == 2)
                    {
                        generarLista(enemigo);
                        enemigo.GetComponent<NavMeshAgent>().destination = nodoDestino.transform.position;
                        
                    }
                }
            }
        }
    }
}
