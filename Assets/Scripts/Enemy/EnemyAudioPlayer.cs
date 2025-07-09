using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioPlayer : NoiseController
{
    [Header("Sonidos")]
    [SerializeField] private AudioClip[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(playRandomSound());
    }

    IEnumerator playRandomSound()
    {
        yield return new WaitForSeconds(Random.Range(10,60));
        PlaySFX(sounds[Random.Range(0, sounds.Length)]);
        StartCoroutine(playRandomSound());
    }
}
