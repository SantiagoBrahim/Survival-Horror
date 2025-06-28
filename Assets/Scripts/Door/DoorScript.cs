using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    public bool isLocked;

    public float openAgle = 90f;
    public float speed = 2f;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    public GameObject bisagraGameObject;

    public float     unlockAttemp;
    public float maxUnlockAttemp = 10;

    public Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        closedRotation = bisagraGameObject.transform.rotation;
        openRotation = Quaternion.Euler(bisagraGameObject.transform.eulerAngles + new Vector3(0f, openAgle, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            bisagraGameObject.transform.rotation = Quaternion.Slerp(bisagraGameObject.transform.rotation, openRotation, Time.deltaTime * speed);
        }
        else
        {
            bisagraGameObject.transform.rotation = Quaternion.Slerp(bisagraGameObject.transform.rotation, closedRotation, Time.deltaTime * speed);
        }
    }

    public void toggleDoor()
    {
        if(!isLocked)
        {
            isOpen = !isOpen;
            // sonido
        }
        else
        {
            unlock();
        }
    }

    public void unlock()
    {
        progressBar.gameObject.SetActive(true);
        unlockAttemp++;
        progressBar.fillAmount = unlockAttemp / maxUnlockAttemp;
        // sonido
        if(unlockAttemp >= maxUnlockAttemp)
        {
            isLocked = false;
            progressBar.color = Color.green;
            toggleDoor();
            StartCoroutine(hideUI());
        }
    }

    IEnumerator hideUI()
    {
        yield return new WaitForSeconds(1);
        progressBar.gameObject.SetActive(false);
    }
}
