using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class DoorScript : NoiseController
{
    public bool isLocked;

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

    [Header("Sonidos")]
    [SerializeField] private float radiusNoise;
    [SerializeField] private AudioClip openDoorSound;
    [SerializeField] private AudioClip knockDoorSound;
    private BoxCollider boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void toggleDoor()
    {
        if(!audioController.isPlaying)
        {
            if (!isLocked)
            {
                anim.Play(openAnimation.name);
                boxCollider.isTrigger = true;
                PlaySFX(openDoorSound);
                MakeNoise(radiusNoise / 1.5f, audioController.gameObject.transform.position);
            }
            else
            {
                unlock();
            }
        }
    }

    public void unlock()
    {
        progressBar.gameObject.SetActive(true);
        unlockAttemp++;
        progressBar.fillAmount = unlockAttemp / maxUnlockAttemp;
        PlaySFX(knockDoorSound);
        MakeNoise(radiusNoise, audioController.gameObject.transform.position);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(audioController.gameObject.transform.position, radiusNoise);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(audioController.gameObject.transform.position, radiusNoise / 1.5f);
    }
}
