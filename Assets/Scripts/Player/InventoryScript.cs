using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using ColorUtility = UnityEngine.ColorUtility;

public class InventoryScript : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject[] objects;

    [Header("Light")]
    public float lightIntensity;
    public float startLightIntensity;

    [Header("Cruz")]
    [SerializeField] private CruzScript cruzScript;

    [Header("HUD")]
    [SerializeField] private Image lightImage;
    [SerializeField] private Image crossImage;
    [SerializeField] private Image LightBGImage;
    [SerializeField] private Image crossBGImage;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }
    }

    private void Update()
    {
        lightImage.fillAmount = lightIntensity / startLightIntensity;
        if(lightIntensity > 0)
        {
            Color newColor;
            ColorUtility.TryParseHtmlString("#D4D4D4", out newColor);
            LightBGImage.color = newColor;
        }
        else
        {
            LightBGImage.color = Color.gray;
        }

        crossImage.fillAmount = cruzScript.timeInCooldown / cruzScript.startCooldown;
        if (cruzScript.timeInCooldown >= cruzScript.startCooldown)
        {
            Color newColor;
            ColorUtility.TryParseHtmlString("#D4D4D4", out newColor);
            crossBGImage.color = newColor;
        }
        else
        {
            crossBGImage.color = Color.gray;
        }
    }

    public void selectLight(InputAction.CallbackContext callback)
    {
        if(callback.performed)
        {
            objects[0].gameObject.SetActive(!objects[0].activeSelf);
            objects[1].gameObject.SetActive(false);
        }
    }

    public void selectCruz(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            objects[0].gameObject.SetActive(false);
            objects[1].gameObject.SetActive(!objects[1].activeSelf);
        }
    }
}
