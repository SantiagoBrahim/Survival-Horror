using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour
{

    private Volume volume;
    private Vignette vignette;

    public bool isHurt;

    // Start is called before the first frame update
    void Start()
    {
        volume = GameObject.FindFirstObjectByType<Volume>();
    }

    public void Hurt()
    {
        if(!isHurt)
        {
            isHurt = true;
            if (volume.profile.TryGet(out vignette))
            {
                vignette.color.Override(Color.red);
            }
            StartCoroutine(timerToHeal(5));
        }
        else
        {
            // game over
            StopAllCoroutines();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Heal()
    {
        if (isHurt)
        {
            isHurt = false;
            if (volume.profile.TryGet(out vignette))
            {
                vignette.color.Override(Color.black);
            }
        }
    }

    IEnumerator timerToHeal(float healTimer)
    {
        yield return new WaitForSeconds(healTimer);
        Heal();
    }

}
