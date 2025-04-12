using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Script_Movimiento : MonoBehaviour
{
    // Fisicas
    private Rigidbody rb;

    // Movimiento
    Vector3 movement;
    Vector2 direction;

    // Velocidad
    [Header("Velocidad")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float crouchSpeed;
    private float moveSpeed;

    // Salto
    [Header("Salto")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radioGroundCheck;
    [SerializeField] private LayerMask groundMask;
    private bool grounded;

    // Stamina
    [Header("Stamina")]
    [SerializeField] private float staminaMax = 100f;
    [SerializeField] private float staminaRemovedWhileRunning;
    [SerializeField] private float staminaChargedWhileResting;
    private float stamina;

    // Movimiento
    private bool isMoving;

    // Correr
    private bool isRunning;

    // Agacharse
    [Header("Agacharse")]
    [SerializeField] private float heightCameraCrouched;
    private float heightCameraNormal;
    private bool crouching;

    // Camara
    [Header("Camara")]
    [SerializeField] private GameObject playerCamera;

    // UI
    [Header("UI")]
    [SerializeField] private GameObject HUD;

    // Stamina
    [Header("Stamina")]
    [SerializeField] private GameObject staminaBar;
    [SerializeField] private Image staminaFillBar1;
    [SerializeField] private Image staminaFillBar2;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = walkSpeed;
        stamina = staminaMax;
        heightCameraNormal = playerCamera.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, radioGroundCheck, groundMask);
        movement = transform.right * direction.x + transform.forward * direction.y;
        rb.velocity = new Vector3(moveSpeed * Time.deltaTime * movement.x, rb.velocity.y, moveSpeed * Time.deltaTime * movement.z);
        if (stamina < 0)
        {
            stamina = 0;
        }
        else if (stamina > staminaMax)
        {
            stamina = staminaMax;
        }

        isMoving = (rb.velocity.x != 0) || (rb.velocity.z != 0);

        if (isMoving && isRunning)
        {
            removeStamina(staminaRemovedWhileRunning * Time.deltaTime);
        }
        else if (stamina < staminaMax && (!isMoving || !isRunning || crouching))
        {
            RechargeStamina(staminaChargedWhileResting * Time.deltaTime);
        }

        if (stamina <= 0)
        {
            moveSpeed = walkSpeed;
            isRunning = false;
        }
    }

    public void Move(InputAction.CallbackContext callback)
    {
        direction = callback.ReadValue<Vector2>();
        // (1, 0) Adelante
        // (-1, 0) Atras
        // (0, -1) Izq
        // (0, 1) Der
    }


    public void Jump(InputAction.CallbackContext callback)
    {
        if(callback.performed && grounded)
        {
            Debug.Log("Salto");
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            Debug.Log(new Vector3(rb.velocity.x, jumpForce, rb.velocity.z));
        }
    }

    public void Run(InputAction.CallbackContext callback)
    {
        if(callback.started && stamina > 0 && !crouching)
        {
            moveSpeed = runSpeed;
            isRunning = true;
        }
        if(callback.canceled)
        {
            moveSpeed = walkSpeed;
            isRunning = false;
        }
    }

    public void Crouch(InputAction.CallbackContext callback)
    {
        if(callback.started)
        {
            crouching = true;
            moveSpeed = crouchSpeed;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, heightCameraCrouched, playerCamera.transform.localPosition.z);
        }
        else if(callback.canceled)
        {
            crouching = false;
            moveSpeed = walkSpeed;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, heightCameraNormal, playerCamera.transform.localPosition.z);
        }
    }

    private void removeStamina(float quantity)
    {
        stamina -= quantity;
        ChangeStaminaUI();
    }

    private void RechargeStamina(float quantity)
    {
        stamina += quantity;
        ChangeStaminaUI();
    }

    private void ChangeStaminaUI()
    {
        staminaFillBar1.fillAmount = stamina / staminaMax;
        staminaFillBar2.fillAmount = stamina / staminaMax;
        ToggleStaminaUI();
    }

    private void ToggleStaminaUI()
    {
        if (stamina >= staminaMax)
        {
            staminaBar.SetActive(false);
        }
        else if (stamina < staminaMax)
        {
            staminaBar.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.transform.position, radioGroundCheck);
    }
}
