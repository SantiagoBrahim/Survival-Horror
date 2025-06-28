using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public float interactionDistance = 3f;
    public Camera playerCamera;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("ACCION");
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                Debug.Log("HIT");
                DoorScript door = hit.collider.gameObject.GetComponent<DoorScript>();
                if (door != null)
                {
                    Debug.Log("PUERTA");
                    door.toggleDoor();
                }
            }
        }
    }
}
