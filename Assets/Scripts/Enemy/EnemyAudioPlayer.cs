using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyAudioPlayer : NoiseController
{
    [Header("Sonidos")]
    [SerializeField] private AudioClip[] sounds;
    public AudioSource walkSound;
    public AudioSource runSound;
    public AudioClip attackAudio;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(playRandomSound());
    }

    IEnumerator playRandomSound()
    {
        yield return new WaitForSeconds(Random.Range(10, 60));
        PlaySFX(sounds[Random.Range(0, sounds.Length)]);
        StartCoroutine(playRandomSound());
    }

    public void PlayFootstepWalkSound()
    {
        walkSound.Play();
    }

    public void PlayFootstepRunSound()
    {
        runSound.Play();
    }

    public void PlayAttackAudio()
    {
        PlaySFX(attackAudio);
    }
}
