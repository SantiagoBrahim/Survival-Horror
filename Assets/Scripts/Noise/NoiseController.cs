using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    public void MakeNoise(float radiusNoise, AudioClip playedSound, Vector3 playPosition)
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
}
