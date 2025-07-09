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
    [SerializeField] private AudioClip lightLoopSound;
    [SerializeField] private AudioClip turnOffSound;
    private float initialVolume;

    [Header("Inventario")]
    [SerializeField] private InventoryScript inventoryScript;

    [Header("Light Game Object")]
    [SerializeField] private GameObject lightGO;

    private void Awake()
    {
        pointLight = GetComponent<Light>();
    }

    private void Start()
    {
        inventoryScript.startLightIntensity = startIntensity;
        initialVolume = audioController.volume;
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
        else if(pointLight.intensity == 0 && audioController.clip == lightLoopSound)
        {
            StopSFX();
        }

        inventoryScript.lightIntensity = pointLight.intensity;

        if(isOn)
        {
            audioController.volume = (pointLight.intensity * initialVolume) / startIntensity;
        }
        else
        {
            audioController.volume = initialVolume;
        }
    }

    public void ToggleLight(InputAction.CallbackContext callback)
    {
        if(callback.performed && isOn && lightGO.activeSelf)
        {
            turnOff();
        }
        else if(callback.performed && !isOn && lightGO.activeSelf)
        {
            turnOn();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusNoise);
    }

    IEnumerator startLoopAudio()
    {
        yield return new WaitForSeconds(turnOnSound.length);
        PlaySFXLoop(lightLoopSound);
    }

    public void turnOn()
    {
        MakeNoise(radiusNoise, transform.position);
        PlaySFX(turnOnSound);
        pointLight.intensity = startIntensity;
        StopAllCoroutines();
        StartCoroutine(startLoopAudio());
    }

    public void turnOff()
    {
        pointLight.intensity = 0;
        PlaySFX(turnOffSound);
        StopAllCoroutines();
    }
}
