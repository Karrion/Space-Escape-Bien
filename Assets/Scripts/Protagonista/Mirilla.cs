using UnityEngine;
using System.Collections;

public class Mirilla : MonoBehaviour {
    public static bool estaApuntando = false;//Para decirle al script del jugador si esta apuntando o no.

    private SpriteRenderer mirillaRenderer;//Imagen de la mirilla
    public SpriteRenderer mirillaPequeñaRenderer;//Imagen de la mirilla cuando apunta a un enemigo
    public GameObject camara;
    public GameObject camaraCerca;//Gameobject que indica la posición de la camara cuando esta alejada
    public GameObject camaraLejos;//Gameobject que indica la posición de la camara cuando esta apuntando(más cerca)
    public bool enemigoApuntado = false;//Si esta apuntando a un enemigo.
    // Use this for initialization
    void Start()
    {
        mirillaRenderer = GetComponent<SpriteRenderer>();
        Cursor.visible = false;
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
                shooting();//Aprobecho el script y disparo xD
            }
            else {
                
                mirillaPequeñaRenderer.enabled = false;
                mirillaRenderer.enabled = true;
                enemigoApuntado = false;
            }
            
    }

    void shooting() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("A dormir");
            //TODO:Completar tirando un gameobject si vemos que queda mejor.
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
                estaApuntando = true;//Para decirle al script del jugador que esta apuntando
            }
        }
        else {
            camara.transform.position = new Vector3(camara.transform.position.x, camara.transform.position.y, camaraLejos.transform.position.z);
            mirillaRenderer.enabled = false;
            estaApuntando = false;//Para decirle al script del jugador que no esta apuntando
            mirillaPequeñaRenderer.enabled = false;
        }
        Vector3 pos = Input.mousePosition;
        // pos.z = transform.position.z - Camera.main.transform.position.z;
        pos.z = 2.0f;
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }

   
}

