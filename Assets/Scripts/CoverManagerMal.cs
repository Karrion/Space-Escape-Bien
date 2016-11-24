using UnityEngine;
using System.Collections;

public class CoverManagerMal : MonoBehaviour {

    GameObject player;
	static GameObject[] coberturas;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        coberturas = new GameObject[transform.childCount];
        for(int i = 0;i < coberturas.Length; i++)
        {
            coberturas[i] = transform.GetChild(i).gameObject;
        }
	}

    public Transform BuscarMasCercana(Transform guardia, float radio)
    {
        Transform resultado = null;
        float minimo = float.MaxValue;
        foreach(GameObject cobertura in coberturas)
        {
            if (!cobertura.GetComponent<Cover>().occupied)
            {
                float distancia = Vector3.Distance(guardia.position, cobertura.transform.position);
                if (distancia < minimo)
                {
                    RaycastHit hit;
                    Vector3 rayDirection = player.transform.position - transform.position;

                    if (Physics.Raycast(transform.position, rayDirection.normalized, out hit))
                    {
                        if (hit.collider.gameObject.tag != "Player")
                        {
                            minimo = distancia;
                            resultado = cobertura.transform;
                        }
                    }
                }
            }
        }

        if (Vector3.Distance(guardia.position, resultado.position) < radio)
        {
            resultado.gameObject.GetComponent<Cover>().occupied = true;
            return resultado;
        }
        else
            return null;
    }
}
