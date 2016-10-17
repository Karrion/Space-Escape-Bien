using UnityEngine;
using System.Collections;

public class Mirilla : MonoBehaviour {
    private SpriteRenderer mirillaRenderer;
    public SpriteRenderer mirillaPequeñaRenderer;
    public GameObject camara;
    public GameObject camaraCerca;
    public GameObject camaraLejos;
    public bool enemigoApuntado = false;
    // Use this for initialization
    void Start()
    {
        mirillaRenderer = GetComponent<SpriteRenderer>();
        
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
            if (hit.collider.gameObject.tag == "Enemy")
            {
                mirillaPequeñaRenderer.enabled = true;
                mirillaRenderer.enabled = false;
              
                Debug.DrawLine(ray.origin, hit.point);
                enemigoApuntado = true;
            }
            else {
                
                mirillaPequeñaRenderer.enabled = false;
                mirillaRenderer.enabled = true;
                enemigoApuntado = false;
            }
            
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButton(1))
        {
            camara.transform.position = new Vector3 (camara.transform.position.x, camara.transform.position.y, camaraCerca.transform.position.z);
            if (!enemigoApuntado)
            {
                mirillaRenderer.enabled = true;
            }
        }
        else {
            camara.transform.position = new Vector3(camara.transform.position.x, camara.transform.position.y, camaraLejos.transform.position.z);
            mirillaRenderer.enabled = false;
			mirillaPequeñaRenderer.enabled = false;
        }
        Vector3 pos = Input.mousePosition;
        // pos.z = transform.position.z - Camera.main.transform.position.z;
        pos.z = 2.0f;
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }
}

