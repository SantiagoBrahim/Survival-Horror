using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Script_FarConeVision : AIStates
{

    public AIEnemy AI;

    public LayerMask obstacleMask;

    Vector3 direccion;

    [SerializeField] private Transform RayCastSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (AI.actualState != States.Stunned && other.CompareTag("Player"))
            TryDetectPlayer(other.transform);
    }

    private void Update()
    {
        if (RayCastSpawn != null && direccion != Vector3.zero)
            Debug.DrawRay(RayCastSpawn.position, direccion * 100, Color.red);
    }

    private void OnTriggerExit(Collider other)
    {
        if (AI.actualState != States.Stunned && other.CompareTag("Player"))
            TryDetectPlayer(other.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        if (AI.actualState != States.Stunned && AI.actualState != States.Chasing && other.CompareTag("Player"))
            TryDetectPlayer(other.transform);
    }

    private void TryDetectPlayer(Transform playerTransform)
    {
        direccion = (playerTransform.position - RayCastSpawn.position).normalized;

        if (!Physics.Raycast(RayCastSpawn.position, direccion, out RaycastHit hit, 100, obstacleMask))
        {
            AI.ChangeState(States.Seeking);
            AI.seekPos = playerTransform.position;
        }
    }
}
