using UnityEngine;
using System.Collections;

public class Habitaciones : MonoBehaviour {


    GameObject enemigo;
    Collider esferaEnemigo;
    public GameObject formacion1;



    void Start()
    {
        enemigo = GameObject.FindGameObjectWithTag("Enemy");
        esferaEnemigo = enemigo.GetComponent<SphereCollider>();
        
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Enemy" && (collider.gameObject.GetComponent<IA>().mode == IA.Mode.Alert || collider.gameObject.GetComponent<IA>().mode == IA.Mode.Shooting))
        {
            Instantiate(formacion1, transform.position, transform.rotation);
            
        }
    }
}
