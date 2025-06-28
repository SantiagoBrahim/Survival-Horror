using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CruzScript : MonoBehaviour
{

    [Header("Cruz en cooldown")]
    public bool inCooldown;

    [Header("Post procesado")]
    public Volume postProcessVolume;
    private Bloom bloom;

    private float targetBloomIntensity;
    private float targetBloomThreshold;

    private float targetExposure;

    private ColorAdjustments colorAdjustments;

    [Header("Materiales")]
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material deactiveMaterial;

    [Header("GameObject")]
    public GameObject cruzGameObject;

    // Start is called before the first frame update
    void Start()
    {
        postProcessVolume.profile.TryGet(out bloom);
        postProcessVolume.profile.TryGet(out colorAdjustments);

        colorAdjustments.postExposure.overrideState = true;

        targetBloomIntensity = 3;
        targetBloomThreshold = 1f;
        targetExposure = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, targetBloomIntensity, Time.deltaTime * 5);
        bloom.threshold.value = Mathf.Lerp(bloom.intensity.value, targetBloomThreshold, Time.deltaTime * 5);
        colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, targetExposure, Time.deltaTime * 5);
    }

    public void diositoSalvadorAmen()
    {
        targetBloomIntensity = 20;
        targetBloomThreshold = 0;
        targetExposure = 20;
        inCooldown = true;
        cruzGameObject.GetComponent<MeshRenderer>().material = deactiveMaterial;
        StartCoroutine(screenEfectTimer());
    }

    IEnumerator screenEfectTimer()
    {
        yield return new WaitForSeconds(1);
        targetBloomIntensity = 3;
        targetBloomThreshold = 1;
        targetExposure = 0;
        StartCoroutine(Cooldown(10));
    }

    IEnumerator Cooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        inCooldown = false;
        cruzGameObject.GetComponent<MeshRenderer>().material = activeMaterial;
    }
}
