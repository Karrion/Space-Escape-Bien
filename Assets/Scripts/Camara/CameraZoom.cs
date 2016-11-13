using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {


    float distanciaOriginal;
    GameObject player;
    bool colisiona = false;
    float distancia;


	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        distanciaOriginal = Vector3.Distance(player.transform.position, transform.position);
    }
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other)
    {
        Debug.Log("eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeh");
        colisiona = true;
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("eeeeeeeeeeeedsfsadfasfsdfaeeeeeeefadsfeeeeeeeeeeeeeeeeeeeeeh");
        colisiona = false;
    }

   void Update()
   {
        Debug.Log(colisiona);
        if (colisiona)
        {
            Debug.Log("hola");
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10f * Time.deltaTime);
        }
        else
        {
            distancia = Vector3.Distance(player.transform.position, transform.position);
            if (distancia > distanciaOriginal)
            {
                Debug.Log("adios");
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z -10f * Time.deltaTime);
            }
        }
    }
}
