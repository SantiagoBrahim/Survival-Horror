using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioController;

    public void MakeNoise(float radiusNoise, Vector3 playPosition)
    {
        Collider[] collisions = Physics.OverlapSphere(playPosition, radiusNoise);

        foreach (var collision in collisions)
        {
            if(collision.gameObject.CompareTag("Enemy"))
            {
                if(collision.gameObject.GetComponent<AIEnemy>())
                {
                    AIEnemy enemyAI = collision.gameObject.GetComponent<AIEnemy>();
                    if(enemyAI.actualState != AIStates.States.Chasing)
                    {
                        enemyAI.ChangeState(AIStates.States.Seeking);
                        enemyAI.seekPos = playPosition;
                    }
                }
            }
        }
    }

    public void PlaySFX(AudioClip sound)
    {
        StopSFX();
        audioController.clip = sound;
        audioController.loop = false;
        audioController.PlayOneShot(sound);
    }

    public void StopSFX()
    {
        audioController.clip = null;
        audioController.loop = false;
        audioController.Stop();
    }

    public void PlaySFXLoop(AudioClip sound)
    {
        audioController.loop = true;
        audioController.clip = sound;
        audioController.Play();
    }
}
