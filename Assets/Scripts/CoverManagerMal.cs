using UnityEngine;
using System.Collections;

public class CoverManagerMal : MonoBehaviour {

	static GameObject[] coberturas;

	void Start () {

        coberturas = new GameObject[transform.childCount];
        for(int i = 0;i < coberturas.Length; i++)
        {
            coberturas[i] = transform.GetChild(i).gameObject;
        }
	}

    public static Transform BuscarMasCercana(Transform guardia, int radio)
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
                    minimo = distancia;
                    resultado = cobertura.transform;
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
