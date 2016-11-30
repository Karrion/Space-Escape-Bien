using UnityEngine;
using System.Collections;

public class Aestrella : MonoBehaviour {

	/*// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
                if (zonaAnterior == 1)
                    nodoInicial = lista[8];
                else if (zonaAnterior == 3)
                    nodoInicial = lista[18];
                break;
            case 3:
                lista = GameObject.FindGameObjectWithTag("Zona3").GetComponentsInChildren<GameObject>();
                nodoInicial = lista[0];
                break;
        }
        listaOpen.Add(nodoInicial);
        contador++;
        nodoDestino = lista[UnityEngine.Random.Range(0, lista.Length)];


        while (contador != 0)
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
    }*/
}
