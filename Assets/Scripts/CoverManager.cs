using UnityEngine;
using System.Collections;
using System;

public class CoverManager : MonoBehaviour {

    public static Transform[] coberturas;
    public static GameObject[] objetos;

    void Awake() {
        coberturas = GetComponentsInChildren<Transform>();
    }

    public static Transform BuscarMasCercana(Transform guardia, int radio)
    {
        Transform coberturaMesPropeta = null;
        float minimo = float.MaxValue;

        for(int i = 0; i < coberturas.Length;i++)
        {
            if (!coberturas[i].gameObject.GetComponent<Cover>().occupied)
            {
                float distancia = Vector3.Distance(guardia.position, coberturas[i].position);
                if (distancia < minimo)
                {
                    minimo = distancia;
                    coberturaMesPropeta = coberturas[i];
                }
            }

        }

        if (Vector3.Distance(guardia.position, coberturaMesPropeta.position) < radio)
            return coberturaMesPropeta;
        else
            return null;
    }
}
