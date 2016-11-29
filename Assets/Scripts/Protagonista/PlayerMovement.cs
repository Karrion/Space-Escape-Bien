using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float Speed = 10f;
    public float TurnSpeed = 180f;
    public float SpeedLimit = 10f;
    public float CrouchLimit = 3f;
    private float initialSpeedLimit;
    private float camRayLength = 100f;

    [HideInInspector]
    public static bool enConducto = false;
    bool agachando = false;
    bool levantando = false;
    bool apuntando = false;


    private string MovementAxisName;
    private string TurnAxisName;
    private Rigidbody Rigidbody;
    private float MovementInputValue;
    private float TurnInputValue;
    private float Acceleration = 40f;
    private Animator anim;
    private MeshRenderer meshRenderer;

    private bool isReloading = false;
    [HideInInspector]
    public static bool Running = false;

    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    public Camera camera;

    //Cosas guays para cambios de zona:
    private GameObject deUnaADos;
    private GameObject deTresADos;



    Mirilla scriptMirilla;

    [HideInInspector]
    public enum Mode { Standing, Crouching, Aiming, Crawling };
    public static Mode mode = Mode.Standing;

    public static Mode getMode()
    {
        return mode;
    }


    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        scriptMirilla = transform.GetChild(0).GetComponent<Mirilla>();
        deUnaADos = GameObject.FindGameObjectWithTag("DesdeZona1");
        deTresADos = GameObject.FindGameObjectWithTag("DesdeZona3");
        deUnaADos.SetActive(false);
        deTresADos.SetActive(false);
    }


    private void OnEnable()
    {
        MovementInputValue = 0f;
        TurnInputValue = 0f;
    }


    private void Start()
    {
        MovementAxisName = "Vertical";
        TurnAxisName = "Horizontal";
        Speed = 0;
        initialSpeedLimit = SpeedLimit;
    }



    void Update()
    {

        MovementInputValue = Input.GetAxis(MovementAxisName);
        TurnInputValue = Input.GetAxis(TurnAxisName);

        //Debug.Log(mode);
        if (!isReloading)
        {
            if (Input.GetKeyDown(KeyCode.C) || (Input.GetKeyDown(KeyCode.LeftControl)))
            {
                if (mode == Mode.Standing)
                {
                    anim.SetBool("Levantarse", false);
                    anim.SetBool("Run", false);
                    anim.SetBool("Aim", false);
                    anim.SetBool("Agachado", true);
                    StartCoroutine("finalizarAgachando");
                    mode = Mode.Crouching;
                }
                else if (mode == Mode.Crouching)
                {
                    anim.SetBool("Run", false);
                    anim.SetBool("Agachado", false);
                    anim.SetBool("CaminarAgachado", false);
                    anim.SetBool("Aim", false);
                    anim.SetBool("Levantarse", true);
                    StartCoroutine("finalizarLevantando");
                    mode = Mode.Standing;
                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            apuntando = true;
            if (mode == Mode.Standing)
            {
                anim.SetBool("Run", false);
                anim.SetBool("Aim", true);
                mode = Mode.Aiming;
            }
            else if (mode == Mode.Crouching)
            {
                anim.SetBool("Agachado", false);
                anim.SetBool("CaminarAgachado", false);
                anim.SetBool("Aim", true);
                anim.SetBool("Levantarse", true);
                mode = Mode.Aiming;
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            apuntando = false;
            anim.SetBool("Aim", false);
            anim.SetBool("Disparar", false);
            mode = Mode.Standing;
        }

        camaraFocus();
    }


    private void FixedUpdate()
    {
        if (!isReloading && !agachando && !levantando)
            Move();
        Turn();
        //turnWhileAim();

    }


    private void Move()
    {
        if (mode == Mode.Standing)
        {
           
            if (MovementInputValue > 0.1f)
            {
                Quaternion currentRotation = GetComponent<Transform>().rotation;
                Running = true;
                anim.SetBool("Run", true);
                Speed = Speed + Acceleration * Time.deltaTime;
                if (Speed > SpeedLimit) Speed = SpeedLimit;
            }
            else if (MovementInputValue == 0)
            {
                Running = false;
                anim.SetBool("Run", false);
                Speed = Speed - Acceleration * 10 * Time.deltaTime;
                if (Speed < 0) Speed = 0;
            }
            else if (MovementInputValue < 0.1f /*&& Running*/)
            {
                Running = true;
                anim.SetBool("Run", true);
                Speed = Speed - Acceleration * Time.deltaTime;
                if (Speed < -SpeedLimit) Speed = -SpeedLimit;
               
            }
        }
        else if (mode == Mode.Crouching)
        {
            if (MovementInputValue > 0.1f)
            {
                anim.SetBool("CaminarAgachado", true);
                Speed = CrouchLimit;
                Speed = Speed + Acceleration * Time.deltaTime;
                if (Speed > CrouchLimit) Speed = CrouchLimit;
            }
            else if (MovementInputValue == 0)
            {
                anim.SetBool("CaminarAgachado", false);
                Speed = 0;
            }
        }
        else
        {
            Speed = 0;
            Apuntar();
            if (Input.GetKey(KeyCode.R))
            {
                anim.SetTrigger("Recargar");
                StartCoroutine("finalizarCarga");
            }
            else if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("Disparar");
                if (scriptMirilla.enemigoApuntado == true)
                {
                    IA scriptEnemigo;
                    scriptEnemigo = scriptMirilla.devolverEnemigo().GetComponent<IA>();
                    Debug.Log(scriptEnemigo.gameObject.name);
                    scriptEnemigo.getHit();
                }
            }
        }

        Vector3 movement = transform.forward * Speed * Time.deltaTime;
        Rigidbody.MovePosition(Rigidbody.position + movement);

    }

    private IEnumerator finalizarCarga()
    {
        isReloading = true;
        yield return new WaitForSeconds(5.4f);
        isReloading = false;

    }

    private IEnumerator finalizarAgachando()
    {
        agachando = true;
        yield return new WaitForSeconds(1.4f);
        agachando = false;

    }

    private IEnumerator finalizarLevantando()
    {
        levantando = true;
        yield return new WaitForSeconds(1.07f);
        levantando = false;

    }

    private void Agacharse()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (MovementInputValue > 0.1f)
            {
                anim.SetBool("agachado", true);
                //particle.SetActive (true);
                Speed = CrouchLimit;
                //runningAudio.Play ();
            }
            else if (MovementInputValue == 0)
            {
                //anim.SetBool("agachado", false);
                //particle.SetActive (false);
                Speed = 0;
                //runningAudio.Stop ();
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetBool("agachado", false);
        }
        
        Vector3 movement = transform.forward * Speed * Time.deltaTime;
        Rigidbody.MovePosition(Rigidbody.position + movement);
    }

    private void Disparar()
    {

    }

    private void Pegar()
    {

    }

    private void Apuntar()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate move direction to pass to character
        if (m_Cam != null && Mirilla.estaApuntando)//Si apunta y se mueve tiene menos velocidad
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(0.1f, 0, 0.1f)).normalized;
            m_Move = v * 0.6f * m_CamForward + h * m_Cam.right;// v * 0.6f, es donde se le baja la velocidad
        }
        else if (Mirilla.estaApuntando)
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * 0.6f * Vector3.forward + h * Vector3.right * 0.1f;// v * 0.6f, es donde se le baja la velocidad
        }
        if (m_Cam != null && !Mirilla.estaApuntando)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;

        }
        else if (!Mirilla.estaApuntando)
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
    }

    void camaraFocus()
    {
        if (apuntando == true && camera.fieldOfView > 37)
        {
            camera.fieldOfView = camera.fieldOfView - 65.0f * Time.deltaTime;
        }

        else if (apuntando == false && camera.fieldOfView < 60)
             {
                camera.fieldOfView = camera.fieldOfView + 65.0f * Time.deltaTime;
             }
    }


    private void Turn()
    {
        float turn = TurnInputValue * TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        Rigidbody.MoveRotation(Rigidbody.rotation * turnRotation);
    }


    private void turnWhileAim()
    {
        if (Input.GetMouseButton(1))
        {
            float h = 7 * Input.GetAxis("Mouse X");
            transform.Rotate(0, h, 0);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "conducto")
        {
            if (mode == Mode.Crouching)
            {
                meshRenderer.enabled = false;

            }
            else if (mode == Mode.Crawling)
            {
                meshRenderer.enabled = true;
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Zona1")
        {
            GameController.zona = 1;
            GameController.zonaAnterior = 2;
            deUnaADos.SetActive(true);
            deTresADos.SetActive(false);
        }
        else if (collider.tag == "DesdeZona1")
            GameController.zonaAnterior = 1;
        else if (collider.tag == "DesdeZona3")
            GameController.zonaAnterior = 3;
        else if (collider.tag == "Zona2")
        {
            GameController.zona = 2;
        }
        else if (collider.tag == "Zona3")
        {
            GameController.zona = 3;
            GameController.zonaAnterior = 2;
            deUnaADos.SetActive(false);
            deTresADos.SetActive(true);
        }
    }

}