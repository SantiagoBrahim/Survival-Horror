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
        // Toma el valor del axis horizontal que puede devolver un rango entre -1 y 1 (-1 si está presionando la tecla "A" y 1 si está presionando la tecla "D")
        float xAxis = Input.GetAxis("Horizontal");

        // Toma el valor del axis vertical que puede devolver un rango entre -1 y 1 (-1 si está presionando la tecla "S" y 1 si está presionando la tecla "W")
        float zAxis = Input.GetAxis("Vertical");


        // Toma la dirección a la que el jugador quiere moverse
        // transform.right indica la derecha del jugador como un vector de 3 direcciones (1,0,0)
        // transform.forward indica el frente del jugador como un vector de 3 direcciones (0,0,1)
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

        if (Input.GetKey(KeyCode.LeftShift) && staminaActual > 0)
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

        if (staminaActual < staminaMax && (!estaMoviendo || !Input.GetKey(KeyCode.LeftShift)))
        {
            RecargarStamina(staminaRecargadaEnReposo * Time.deltaTime);
        }

        Debug.Log(velocidadJugador);
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
}
