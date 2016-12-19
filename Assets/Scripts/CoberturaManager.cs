using UnityEngine;
using System.Collections;

public class CoberturaManager : MonoBehaviour {

    public bool coberturaSegura = false;
    private GameObject player;
    public int zonaCobertura;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (GameController.zona == zonaCobertura) {
            RaycastHit hit;
            Vector3 rayDirection = player.transform.position - transform.position;

            if (Physics.Raycast(transform.position, rayDirection.normalized, out hit,10))
            {
                Debug.DrawLine(transform.position + transform.up, hit.point);
                if (hit.collider.gameObject.tag != "Player")
                {
                    coberturaSegura = true;
                }
                else
                {
                    coberturaSegura = false;
                }
                
            }
        }
    }
}
