using UnityEngine;
using System.Collections;

public class Deteccion : MonoBehaviour {
    IA IaPadre;
    GameObject player;
    SphereCollider sphere;
    NavMeshAgent agent;
    IAManagement iam;
    private bool playerDentro = false;

	// Use this for initialization
	void Start () {
        IaPadre = transform.GetComponentInParent<IA>();
        player = GameObject.FindGameObjectWithTag("Player");
        sphere = GetComponent<SphereCollider>();
        agent = transform.GetComponentInParent<NavMeshAgent>();
        iam = GameObject.FindGameObjectWithTag("IaManagement").GetComponent<IAManagement>();

	}

    public void OnTriggerStay(Collider other)
    {
        if (!IaPadre.pupa)
        {
            if (!IaPadre.muerto)
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
                                IaPadre.inSight = true;
                                //Debug.Log("Te veo");
                                if (IaPadre.mode != IA.Mode.Shooting)
                                {
                                    if (IaPadre.points.Length != 0) IaPadre.currentPatrol = agent.destination;
                                    IaPadre.mode = IA.Mode.Alert;

                                }
                            }
                            else
                            {
                                
                                IaPadre.inSight = false;
                                //Debug.Log("No te veo");
                                if (IaPadre.mode == IA.Mode.Shooting)
                                    IaPadre.mode = IA.Mode.Alert;
                                
                            }
                        }
                        else
                        {
                            IaPadre.inSight = false;
                        }
                    }
                    else if (PlayerMovement.Running && (IaPadre.mode == IA.Mode.Patrol || IaPadre.mode == IA.Mode.Alert || IaPadre.mode == IA.Mode.Tapon))
                    {
                        IaPadre.escuchado();
                    }
                    else if (PlayerMovement.Running && IaPadre.mode == IA.Mode.Search)
                    {
                        IaPadre.escuchaBuscando = true;
                        agent.destination = player.transform.position;
                    }
                    if ((IaPadre.mode == IA.Mode.Shooting) && (PlayerMovement.Running || PlayerMovement.apuntando))
                    {
                        IaPadre.disparar = true;
                    }
                }

                else if (other.tag == "Enemy" && !playerDentro)
                {
                    buscaMuertos(other);
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") playerDentro = true; 
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") playerDentro = false;
    }

    public void buscaMuertos(Collider collider)
    {
         if ( collider.gameObject.GetComponent<IA>().muerto)
         {
               IaPadre.agent.destination = collider.gameObject.transform.position;
               if (IaPadre.agent.remainingDistance < 2f) {
                    collider.gameObject.GetComponent<IA>().vida = 3;
                    collider.gameObject.GetComponent<IA>().muerto = false;
                    collider.gameObject.GetComponent<IA>().anim.SetBool("Caminar", true);
                    collider.gameObject.GetComponent<IA>().anim.SetBool("Caminar", true);
                    collider.gameObject.GetComponent<IA>().mode = IA.Mode.Patrol;
                    collider.gameObject.GetComponent<IA>().agent.Resume();                                      
            }
        }
    }
    
}
