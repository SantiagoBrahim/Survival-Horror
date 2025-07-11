using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CruzScript : NoiseController
{

    [Header("Cruz en cooldown")]
    public bool inCooldown;
    public float timeInCooldown;
    public float startCooldown;

    [Header("Post procesado")]
    public Volume postProcessVolume;
    private Bloom bloom;

    private float targetBloomIntensity;

    private float targetExposure;

    private ColorAdjustments colorAdjustments;

    [Header("Materiales")]
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material deactiveMaterial;

    [Header("GameObject")]
    public GameObject cruzGameObject;

    [Header("SFX")]
    [SerializeField] private AudioClip flashSound;
    [SerializeField] private AudioClip cruzCargadaLoop;

    // Start is called before the first frame update
    void Start()
    {
        postProcessVolume.profile.TryGet(out bloom);
        postProcessVolume.profile.TryGet(out colorAdjustments);

        colorAdjustments.postExposure.overrideState = true;

        targetBloomIntensity = 3;
        targetExposure = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, targetBloomIntensity, Time.deltaTime * 5);
        colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, targetExposure, Time.deltaTime * 5);
    }

    public void diositoSalvadorAmen()
    {
        PlaySFX(flashSound);
        targetBloomIntensity = 20;
        targetExposure = 20;
        inCooldown = true;
        cruzGameObject.GetComponent<MeshRenderer>().material = deactiveMaterial;
        StartCoroutine(screenEfectTimer());
    }

    IEnumerator screenEfectTimer()
    {
        yield return new WaitForSeconds(1);
        targetBloomIntensity = 3;
        targetExposure = 0;
        StartCoroutine(Cooldown(startCooldown));
    }

    IEnumerator Cooldown(float cooldown)
    {
        timeInCooldown = 0;
        while (timeInCooldown < cooldown)
        {
            yield return null;
            timeInCooldown += Time.deltaTime;
        }
        inCooldown = false;
        PlaySFXLoop(cruzCargadaLoop);
        cruzGameObject.GetComponent<MeshRenderer>().material = activeMaterial;
    }

}
