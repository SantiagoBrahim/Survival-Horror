using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SC_Movimiento : MonoBehaviour
{

    private  CharacterController playerController;

    // Velocidad
    [Header("Velocidad")]
    public float velocidadJugadorCaminando;
    public float velocidadJugadorCorriendo;
    public float velocidadJugadorAgachado;
    private float velocidadJugador;

    // Salto
    [Header("Salto")]
    [SerializeField] private float alturaSalto;

    // Gravedad
    [Header("Gravedad")]
    [SerializeField] private float gravedad = -9.81f;
    public Transform groundCheck;
    public float radioGroundCheck = 0.4f;
    public LayerMask groundMask;
    bool enSuelo;
    private Vector3 velocidadGravedad;

    // Stamina
    [Header("Stamina")]
    [SerializeField] private float staminaMax = 100f;
    [SerializeField] private float staminaQuitadaAlCorrer;
    [SerializeField] private float staminaRecargadaEnReposo;
    private float staminaActual;

    // Movimiento
    private bool estaMoviendo;

    // Agacharse
    [Header("Agacharse")]
    [SerializeField] private float alturaCamaraAgachado;
    private float alturaCamaraNormal;
    private bool agachado;

    // Camara
    [Header("Camara")]
    [SerializeField] private GameObject camara;

    // UI
    [Header("UI")]
    [SerializeField] private GameObject HUD;

    // Stamina
    [Header("Stamina")]
    [SerializeField] private GameObject barraStaminaCompleta;
    [SerializeField] private Image barraStamina1;
    [SerializeField] private Image barraStamina2;


    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        alturaCamaraNormal = camara.transform.localPosition.y;
        velocidadJugador = velocidadJugadorCaminando;
        staminaActual = staminaMax;
    }

    // Update is called once per frame
    void Update()
    {
        enSuelo = Physics.CheckSphere(groundCheck.position, radioGroundCheck, groundMask);

        // Movimiento
        Movement();

        // Gravedad
        GravityController();

        // Salto
        JumpCheck();

        // Correr
        RunCheck();

        // Agacharse
        CrouchCheck();

    }


    private void JumpCheck()
    {
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            velocidadGravedad.y = Mathf.Sqrt(alturaSalto * -2f * gravedad);
        }
    }

    private void GravityController()
    {
        if (enSuelo && velocidadGravedad.y < 0)
        {
            velocidadGravedad.y = -2f;
        }

        velocidadGravedad.y += gravedad * Time.deltaTime;

        playerController.Move(velocidadGravedad * Time.deltaTime);
    }

    private void Movement()
    {
        // Toma el valor del axis horizontal que puede devolver un rango entre -1 y 1 (-1 si est� presionando la tecla "A" y 1 si est� presionando la tecla "D")
        float xAxis = Input.GetAxis("Horizontal");

        // Toma el valor del axis vertical que puede devolver un rango entre -1 y 1 (-1 si est� presionando la tecla "S" y 1 si est� presionando la tecla "W")
        float zAxis = Input.GetAxis("Vertical");


        // Toma la direcci�n a la que el jugador quiere moverse
        // transform.right indica la derecha del jugador como un vector de 3 direcciones
        // transform.forward indica el frente del jugador como un vector de 3 direcciones
        Vector3 movimiento = transform.right * xAxis + transform.forward * zAxis;

        // Funcion para mover al jugador
        // Se usa la direccion a la que se quiere ir multiplicada por la velocidad del jugador y el Time.deltaTime para que se normalice la velocidad independientemente del framerate
        playerController.Move(movimiento * velocidadJugador * Time.deltaTime);
        estaMoviendo = (transform.right * xAxis + transform.forward * zAxis) != Vector3.zero;
    }

    private void RunCheck()
    {
        if (staminaActual < 0)
        {
            staminaActual = 0;
        }
        else if(staminaActual > staminaMax)
        {
            staminaActual = staminaMax;
        }

        if (Input.GetKey(KeyCode.LeftShift) && staminaActual > 0 && !agachado)
        {
            velocidadJugador = velocidadJugadorCorriendo;
            if(estaMoviendo)
            {
                QuitarStamina(staminaQuitadaAlCorrer * Time.deltaTime);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || staminaActual <= 0)
        {
            velocidadJugador = velocidadJugadorCaminando;
        }

        if (staminaActual < staminaMax && (!estaMoviendo || !Input.GetKey(KeyCode.LeftShift) || agachado))
        {
            RecargarStamina(staminaRecargadaEnReposo * Time.deltaTime);
        }
    }

    private void QuitarStamina(float staminaQuitada)
    {
        staminaActual -= staminaQuitada;
        CambiarUIStamina();
    }

    private void RecargarStamina(float staminaSumada)
    {
        staminaActual += staminaSumada;
        CambiarUIStamina();
    }

    private void CambiarUIStamina()
    {
        barraStamina1.fillAmount = staminaActual / staminaMax;
        barraStamina2.fillAmount = staminaActual / staminaMax;
        ToggleStaminaUI();
    }

    private void ToggleStaminaUI()
    {
        if(staminaActual >= staminaMax)
        {
            barraStaminaCompleta.SetActive(false);
        }
        else if(staminaActual < staminaMax)
        {
            barraStaminaCompleta.SetActive(true);
        }
    }

    private void CrouchCheck()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            agachado = true;
            velocidadJugador = velocidadJugadorAgachado;
            camara.transform.localPosition = new Vector3(camara.transform.localPosition.x, alturaCamaraAgachado, camara.transform.localPosition.z);
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            agachado = false;
            camara.transform.localPosition = new Vector3(camara.transform.localPosition.x, alturaCamaraNormal, camara.transform.localPosition.z);
            velocidadJugador = velocidadJugadorCaminando;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.transform.position, radioGroundCheck);
    }

}
