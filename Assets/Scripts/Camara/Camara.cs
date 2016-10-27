using UnityEngine;
using System.Collections;

public class Camara : MonoBehaviour {
    private bool modoAlerta = false;//Para darselo al gamemanager cuando se pueda y que ponga los soldados en modo alerta al ver al personaje.
    public GameObject objetivo;

	// Use this for initialization
	void Start () {
	
	}

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.up);
        ray.origin = transform.position;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point);
            if (hit.collider.gameObject.tag == "Player")
            {
                modoAlerta = true;
                Debug.Log("Visto");
            }
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
