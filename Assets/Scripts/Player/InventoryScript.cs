using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryScript : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var obj in objects)
        {
            obj.SetActive(false);
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
