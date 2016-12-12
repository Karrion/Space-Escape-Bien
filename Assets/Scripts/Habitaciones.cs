using UnityEngine;
using System.Collections;

public class Habitaciones : MonoBehaviour {


    GameObject enemigo;
    Collider esferaEnemigo;
    public GameObject formacion1;
    private IAManagement iam;


    void Start()
    {
        enemigo = GameObject.FindGameObjectWithTag("Enemy");
        esferaEnemigo = enemigo.GetComponent<SphereCollider>();
        iam = GameObject.FindGameObjectWithTag("IaManagement").GetComponent<IAManagement>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Enemy" && (collider.gameObject.GetComponent<IA>().mode == IA.Mode.Alert || collider.gameObject.GetComponent<IA>().mode == IA.Mode.Shooting))
        {
            iam.EnemigoEnSala(collider.gameObject, transform.GetChild(0).gameObject, transform.GetChild(1).gameObject);
        }
    }
}
