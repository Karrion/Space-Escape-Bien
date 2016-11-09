using UnityEngine;
using System.Collections;
using System;

public class CoverManager : MonoBehaviour {
    public static Transform[] coberturas;

    // Use this for initialization
    void Start() {
        coberturas = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update() {

    }

    public static Transform BuscarMasCercana(Transform guardia)
    {
        Transform coberturaMesPropeta = null;
        float minimo = float.MaxValue;
        foreach(Transform cobertura in coberturas)
        {
            float distancia = Vector3.Distance(guardia.position, cobertura.position);
            if (distancia < minimo)
            {
                minimo = distancia;
                coberturaMesPropeta = cobertura;
            }

        }
        return coberturaMesPropeta;
    }
}
