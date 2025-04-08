using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_VistaMouse : MonoBehaviour
{
    // Sensibilidad del Mouse
    [Header("Sensibilidad del Mouse")]
    public float sensibilidadMouseX = 100f;
    public float sensibilidadMouseY = 100f;

    // Jugador
    [Header("Jugador")]
    public Transform playerTransform;


    private float rotacionX = 0f;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouseX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouseY * Time.deltaTime;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        playerTransform.Rotate(Vector3.up * mouseX);

    }
}
