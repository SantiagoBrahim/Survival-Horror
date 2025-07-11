using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            // Ganar
            Debug.Log("Ganaste");
        }
    }
}
