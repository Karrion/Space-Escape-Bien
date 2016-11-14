using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {
    bool abrir;
    GameObject enemigo;
    Collider esferaEnemigo;
    int contador = 0;
    float Yinicial;
	// Use this for initialization
	void Start()
    {
        abrir = false;
        enemigo = GameObject.FindGameObjectWithTag("Enemy");
        esferaEnemigo = enemigo.GetComponent<SphereCollider>();
        Yinicial = transform.position.y;
      
    }
	
	// Update is called once per frame
	void Update () {
        if (abrir)
        {
            if (transform.position.y - Yinicial >= -4.5)
            {
                transform.Translate((3 * Vector3.down) * Time.deltaTime);
              
            }
         
        }
        else {
            if (transform.position.y < Yinicial)
            {
                transform.Translate((3 * Vector3.up) * Time.deltaTime);
              
            }
        }
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag != "EsferaEnemiga")
        {
            abrir = true;
            contador++;           
        }
         
    }
   
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag != "EsferaEnemiga")
        {
            if (contador > 0)
                contador--;
            if (contador == 0)
                abrir = false;
        }

    }
}
