using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    public bool isLocked;

    private bool isOpen = false;

    public GameObject bisagraGameObject;

    public float     unlockAttemp;
    public float maxUnlockAttemp = 10;

    public Image progressBar;
    public Material unlockMat;
    public Material lockMat;

    [SerializeField] private PointLight pointLight;

    [Header("Animaciones")]
    [SerializeField] private Motion openAnimation;
    [SerializeField] private Motion knockAnimation;
    private Animator anim;

    private BoxCollider boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void toggleDoor()
    {
        if(!isLocked)
        {
            //isOpen = !isOpen;

            anim.Play(openAnimation.name);
            boxCollider.isTrigger = true;
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
        anim.Play(knockAnimation.name);

        if(unlockAttemp >= maxUnlockAttemp)
        {
            isLocked = false;
            progressBar.material = unlockMat;
            pointLight.color = LinearColor.Convert(Color.green, 1);
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
