using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{


    float distanciaOriginal;
    GameObject player;
    bool colisiona = false;
    float distancia;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        distanciaOriginal = Vector3.Distance(transform.parent.position, transform.position);
    }

    public void OnCollisionStay(Collision collision)
    {
        colisiona = true;
    }

    public void OnCollisionExit(Collision collision)
    {
        colisiona = false;
    }

    void LateUpdate()
    {
        if (colisiona)
        {
            Debug.Log("choco");
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, transform.parent.position, 0.5f * Time.deltaTime);
        }
        else
        {
            distancia = Vector3.Distance(transform.parent.position, transform.position);
            if (distancia < distanciaOriginal)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10f * Time.deltaTime);
            }
        }
    }
}
