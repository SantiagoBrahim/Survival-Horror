using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Script_Camera : MonoBehaviour
{

    // Sensibilidad del Mouse
    [Header("Sensibilidad del Mouse")]
    public float sensibilidadMouseX = 100f;
    public float sensibilidadMouseY = 100f;

    // Jugador
    [Header("Jugador")]
    public Transform playerTransform;


    private float rotacionX = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Look(InputAction.CallbackContext callback)
    {
        Debug.Log(callback.ReadValue<Vector2>());

        float mouseX = callback.ReadValue<Vector2>().x * sensibilidadMouseX * Time.deltaTime;
        float mouseY = callback.ReadValue<Vector2>().y * sensibilidadMouseY * Time.deltaTime;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        playerTransform.Rotate(Vector3.up * mouseX);
    }




}
