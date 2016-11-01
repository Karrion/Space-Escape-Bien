using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{       
    public float Speed = 10f;            
    public float TurnSpeed = 180f;
    public float SpeedLimit = 10f;
    public float WalkLimit = 5f;
    public float CrouchLimit = 3f;
    private float initialSpeedLimit;

    private string MovementAxisName;     
    private string TurnAxisName;         
    private Rigidbody Rigidbody;         
    private float MovementInputValue;    
    private float TurnInputValue;        
    private float Acceleration = 40f;
    private Animator anim;
	//public GameObject particle;
	//public GameObject particleVelocidad;
	public static AudioSource runningAudio;

    //public Rigidbody Shell;
    //public Transform FireTransform;
    private string FireButton;
    private bool Fired;
    private bool Sprint = false;
    private float SprintTimer;
    private float SprintCooldown;

    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }


    private void OnEnable ()
    {
        MovementInputValue = 0f;
        TurnInputValue = 0f;
    }


    private void Start()
    {
        MovementAxisName = "Vertical";
        TurnAxisName = "Horizontal";
        FireButton = "Fire1";
        Speed = 0;
		runningAudio = GetComponent<AudioSource> ();
		//particle.SetActive (false);
		//particleVelocidad.SetActive (false);
        initialSpeedLimit = SpeedLimit;
    }
    
    private void Update()
    {

        MovementInputValue = Input.GetAxis(MovementAxisName);
        TurnInputValue = Input.GetAxis(TurnAxisName);

        
        

    }

    //private void Fire()
    //{
    //    Fired = true;
    //    Vector3 adjust = new Vector3(0, 0, 0);
    //    Rigidbody shellInstance = Instantiate(Shell, FireTransform.position, FireTransform.rotation) as Rigidbody;
    //    shellInstance.velocity = 40f * FireTransform.forward;
    //}

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            Agacharse();
        else
        {
            Move();
            Turn();
        }
    }


    private void Move()
    {   
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (MovementInputValue > 0.1f)
            {
                if (anim.GetBool("Walk") == true)
                    anim.SetBool("Walk", false);
                anim.SetBool("Run", true);
                //particle.SetActive (true);
                Speed = Speed + Acceleration * Time.deltaTime;
                if (Speed > SpeedLimit) Speed = SpeedLimit;
                //runningAudio.Play ();
            }
            else if (MovementInputValue == 0)
            {
                anim.SetBool("Run", false);
                //particle.SetActive (false);
                Speed = Speed - Acceleration * 10 * Time.deltaTime;
                if (Speed < 0) Speed = 0;
                //runningAudio.Stop ();
            }
            else
            {
                anim.SetBool("Run", true);
                //particle.SetActive (true);
                Speed = Speed - Acceleration * Time.deltaTime;
                if (Speed < -SpeedLimit) Speed = -SpeedLimit;

            }
        }

        else
        {
            if (MovementInputValue > 0.1f)
            {
                if(anim.GetBool("Run") == true)
                    anim.SetBool("Run", false);
                anim.SetBool("Walk", true);
                Speed = WalkLimit;
                //particle.SetActive (true);
                /*Speed = Speed + Acceleration * Time.deltaTime;
                if (Speed > WalkLimit) Speed = SpeedLimit;
                //runningAudio.Play ();*/
            }
            else if (MovementInputValue == 0)
            {
                anim.SetBool("Walk", false);
                Speed = 0;
                //particle.SetActive (false);
                /*Speed = Speed - Acceleration * 10 * Time.deltaTime;
                if (Speed < 0) Speed = 0;
                //runningAudio.Stop ();*/
            }
           /* else
            {
                anim.SetBool("Walk", true);
                //particle.SetActive (true);
                Speed = Speed - Acceleration * Time.deltaTime;
                if (Speed < -WalkLimit) Speed = -SpeedLimit;
                //runningAudio.Play();
            }*/
        }

        Vector3 movement = transform.forward * Speed * Time.deltaTime;
        Rigidbody.MovePosition(Rigidbody.position + movement);
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
       /* else
        {
            anim.SetBool("agachado", true);
            //particle.SetActive (true);
            Speed = Speed - Acceleration * Time.deltaTime;
            if (Speed < -CrouchLimit) Speed = -SpeedLimit;
            //runningAudio.Play();
        }*/
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
        else if (m_Cam != null && !Mirilla.estaApuntando)
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


    private void Tumbarse()
    {

    }

    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.


        float turn = TurnInputValue * TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        Rigidbody.MoveRotation(Rigidbody.rotation * turnRotation);
    }

}