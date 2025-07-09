using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Script_NearVisionCone : AIStates
{
    public AIEnemy AI;

    public LayerMask obstacleMask;

    float distance;
    Vector3 direccion;

    [SerializeField] private Transform RayCastSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            TrySeePlayer(other.transform, States.Chasing);
    }

    private void Update()
    {
        if (RayCastSpawn != null && direccion != Vector3.zero && distance > 0)
            Debug.DrawRay(RayCastSpawn.position, direccion * distance, Color.red);
    }

private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            TrySeePlayer(other.transform, States.Seeking);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            TrySeePlayer(other.transform, States.Chasing);
    }

    private void TrySeePlayer(Transform player, States newState)
    {
        direccion = (player.position - RayCastSpawn.position).normalized;
        distance = Vector3.Distance(RayCastSpawn.position, player.position);

        if (!Physics.Raycast(RayCastSpawn.position, direccion, out RaycastHit hit, distance, obstacleMask))
        {
            if (AI.actualState != States.Stunned && AI.actualState != States.Attacking)
            {
                AI.ChangeState(newState);
                if (newState == States.Seeking)
                    AI.seekPos = player.position;
            }
        }
    }
}
