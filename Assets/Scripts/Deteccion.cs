using UnityEngine;
using System.Collections;

public class Deteccion : MonoBehaviour {
    IA IaPadre;
    GameObject player;
    SphereCollider sphere;
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        IaPadre = transform.GetComponentInParent<IA>();
        player = GameObject.FindGameObjectWithTag("Player");
        sphere = GetComponent<SphereCollider>();
        agent = transform.GetComponentInParent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            IaPadre.inSight = false;
            RaycastHit hit;
            Vector3 rayDirection = player.transform.position - transform.position + transform.up;
            if ((Vector3.Angle(rayDirection, transform.forward)) <= IaPadre.fieldOfViewDegrees * 0.5f)
            {
                if (Physics.Raycast(transform.position + transform.up, rayDirection.normalized, out hit, sphere.radius))
                {
                    Debug.DrawLine(transform.position + transform.up, hit.point);
                    if (hit.transform.CompareTag("Player"))
                    {
                        if (IaPadre.mode != IA.Mode.Shooting)
                        {
                            IaPadre.currentPatrol = agent.destination;
                            IaPadre.mode = IA.Mode.Alert;
                            IaPadre.inSight = true;
                            agent.autoBraking = true;
                        }
                    }
                    else
                    {
                        if (IaPadre.mode == IA.Mode.Shooting)
                            IaPadre.mode = IA.Mode.Alert;

                    }
                }
                else
                {
                    IaPadre.inSight = false;
                }
            }
            else if (PlayerMovement.Running && IaPadre.mode == IA.Mode.Patrol)
            {
                IaPadre.escuchado();
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
            if (PlayerMovement.Running && IaPadre.mode == IA.Mode.Patrol)
            {
                /* currentPatrol = agent.destination;
                 agent.destination = player.transform.position;
                 if(agent.remainingDistance >= 0)
                 {
                     StartCoroutine("tiempoEspera");

                 }*/
                IaPadre.escuchado();
            }
    }
}
