using UnityEngine;
using System.Collections;

public class CoverManagerMal : MonoBehaviour {

	static GameObject[] m_GameObjects;
	void Start () {
        m_GameObjects = new GameObject[transform.childCount];
        for(int i = 0;i < m_GameObjects.Length; i++)
        {
            m_GameObjects[i] = transform.GetChild(i).gameObject;
        }
	}

    public static Transform BuscarMasCercana(Transform guardia, int radio)
    {
        Transform resultado = null;
        float minimo = float.MaxValue;
        foreach(GameObject objeto in m_GameObjects)
        {
            if (!objeto.GetComponent<Cover>().occupied)
            {
                float distancia = Vector3.Distance(guardia.position, objeto.transform.position);
                if (distancia < minimo)
                {
                    minimo = distancia;
                    resultado = objeto.transform;
                }
            }
        }
        if (Vector3.Distance(guardia.position, resultado.position) < radio)
            return resultado;
        else
            return null;
    }
        
       
}
