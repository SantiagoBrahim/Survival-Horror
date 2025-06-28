using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Script_NearVisionCone : AIStates
{
    public AIEnemy AI;

    public LayerMask obstacleMask;

    float distance = 100;
    Vector3 direccion;

    [SerializeField] private Transform RayCastSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            distance = Vector3.Distance(transform.position, other.transform.position);
            direccion = (other.transform.position - RayCastSpawn.position).normalized;
            if (!Physics.Raycast(RayCastSpawn.position, direccion, out RaycastHit hit, 100, obstacleMask))
            {
                if (AI.actualState != States.Stunned)
                {
                    AI.ChangeState(States.Chasing);
                    Debug.Log("CHASING 1");
                }
            }
        }
    }

    private void Update()
    {
        if (direccion != null)
        {
            Debug.DrawRay(RayCastSpawn.position, direccion * distance, Color.red);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            distance = Vector3.Distance(transform.position, other.transform.position);
            direccion = (other.transform.position - RayCastSpawn.position).normalized;
            if (!Physics.Raycast(RayCastSpawn.position, direccion, out RaycastHit hit, 100, obstacleMask))
            {
                if (AI.actualState != States.Stunned)
                {
                    AI.ChangeState(States.Seeking);
                    AI.seekPos = other.transform.position;
                    Debug.Log("SEEKING");
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            distance = Vector3.Distance(transform.position, other.transform.position);
            direccion = (other.transform.position - RayCastSpawn.position).normalized;
            if (!Physics.Raycast(RayCastSpawn.position, direccion, out RaycastHit hit, 100, obstacleMask))
            {
                if (AI.actualState != States.Stunned && AI.actualState != States.Attacking)
                {
                    AI.ChangeState(States.Chasing);
                    Debug.Log("CHASING 1");
                }
            }
        }
    }
}
