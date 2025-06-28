using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Script_FarConeVision : AIStates
{

    public AIEnemy AI;

    public LayerMask obstacleMask;

    float distance = 100;
    Vector3 direccion;

    [SerializeField] private Transform RayCastSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if(AI.actualState != States.Stunned)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                distance = Vector3.Distance(transform.position, other.transform.position);
                direccion = (other.transform.position - RayCastSpawn.position).normalized;
                if (!Physics.Raycast(RayCastSpawn.position, direccion, out RaycastHit hit, 100, obstacleMask))
                {
                    AI.ChangeState(States.Seeking);
                    AI.seekPos = other.transform.position;
                }
            }
        }
    }

    private void Update()
    {
        if(direccion != null)
        {
            Debug.DrawRay(RayCastSpawn.position, direccion * 100, Color.red);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (AI.actualState != States.Stunned)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                distance = Vector3.Distance(transform.position, other.transform.position);
                direccion = (other.transform.position - RayCastSpawn.position).normalized;
                if (!Physics.Raycast(RayCastSpawn.position, direccion, out RaycastHit hit, 100, obstacleMask))
                {
                    AI.ChangeState(States.Seeking);
                    AI.seekPos = other.transform.position;
                }
            }
        }
    }
}
