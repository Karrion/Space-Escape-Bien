using UnityEngine;
using System.Collections;

public class IAManagement : MonoBehaviour {
    Transform objectivePosition;
    Transform playerPosition;
    private int zona = 1;
    GameObject[] guardias;


	// Use this for initialization
	void Start () {
        guardias = GameObject.FindGameObjectsWithTag("Enemy");
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
                if (enemigo.GetComponent<IA>().zonaEnemigo == guardia.GetComponent<IA>().zona) {
                    enemigo.GetComponent<IA>().switchGenerarListas();
                    enemigo.GetComponent<IA>().mode = IA.Mode.Search;
                    //enemigo.GetComponent<NavMeshAgent>().destination = playerPosition.position;
                }
                if (enemigo.GetComponent<IA>().zonaEnemigo != guardia.GetComponent<IA>().zona)
                {
                    enemigo.GetComponent<IA>().mode = IA.Mode.Tapon;
                }
            }
        }
    }
}
