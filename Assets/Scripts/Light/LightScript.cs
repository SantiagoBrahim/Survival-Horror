using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightScript : NoiseController
{
    private Light pointLight;

    [Header("Intensidad Inicial")]
    [SerializeField] private float startIntensity;

    private bool isOn;

    [Header("Porcentaje que se quita a la luz por segundo")]
    [SerializeField] float minusPercentagePerSecond;

    [Header("Sonido")]
    [SerializeField] private float radiusNoise;
    [SerializeField] private AudioClip turnOnSound;

    [Header("Inventario")]
    [SerializeField] private InventoryScript inventoryScript;

    private void Awake()
    {
        pointLight = GetComponent<Light>();
    }

    private void Start()
    {
        inventoryScript.startLightIntensity = startIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        isOn = pointLight.intensity > 0;
        float minusQuantity = (minusPercentagePerSecond / 100) * startIntensity;

        if (pointLight.intensity > 0)
        {
            pointLight.intensity -= minusQuantity * Time.deltaTime;
        }

        inventoryScript.lightIntensity = pointLight.intensity;
    }

    public void ToggleLight(InputAction.CallbackContext callback)
    {
        if(callback.performed && isOn)
        {
            pointLight.intensity = 0;
        }
        else if(callback.performed && !isOn)
        {
            MakeNoise(radiusNoise, turnOnSound, transform.position);
            pointLight.intensity = startIntensity;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusNoise);
    }
}
