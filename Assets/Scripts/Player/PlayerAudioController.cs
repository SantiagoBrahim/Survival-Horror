using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : NoiseController
{
    public AudioClip runStepsSound;
    public AudioClip walkStepsSound;
    public AudioClip fastBreathSound;
    public AudioClip slowBreathSound;
    public float radiusNoise;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusNoise);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusNoise / 1.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusNoise * 1.1f);
    }
}
