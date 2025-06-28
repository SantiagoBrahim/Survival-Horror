using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Random = UnityEngine.Random;
using System.Collections;
using System;

public class AIEnemy : AIStates
{
    // Velocidad
    [Header("Velocidad")]
    public float speed;

    // Objetivo
    [Header("Objetivo")]
    public Transform target;

    // NavMesh
    [Header("NavMesh")]
    [SerializeField] private NavMeshSurface navMesh;
    [SerializeField] private float sampleRadius = 1f;
    private Vector3 navMeshSize;
    private Vector3 navMeshCenter;
    private Vector3 navMeshHalfSize;
    private Transform surfaceTransform;
    private NavMeshAgent AI;

    // IA
    private bool moving;

    //Wandering
    [Header("Wandering")]
    [SerializeField] private Transform frontLeftAreaTransform;
    [SerializeField] private Transform backRightAreaTransform;


    // Seeking
    [Header("Seeking")]
    public Vector3 seekPos;

    // Vision Cone
    [Header("Vision Cone")]
    [SerializeField] GameObject farVisionCone;
    [SerializeField] GameObject nearVisionCone;
    [SerializeField] GameObject peripherealLeftVisionCone;
    [SerializeField] GameObject peripherealRightVisionCone;

    // Ataque
    private bool inCooldownAttack;

    private void Awake()
    {
        AI = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        ChangeState(States.Idle);
        AI.speed = speed;
        navMeshSize = navMesh.size;
        navMeshCenter = navMesh.center;
        navMeshHalfSize = navMeshSize * 0.5f;
        surfaceTransform = navMesh.transform;
        moving = false;
        StartCoroutine(ChangeIdleWanderingState(3));

    }

    private void Update()
    {
        switch(actualState)
        {
            case States.Idle:
                AI.isStopped = true;
            break;
            case States.Wandering:
                AI.isStopped = false;
                if (!moving)
                {
                    float randX = Random.Range(frontLeftAreaTransform.position.x, backRightAreaTransform.position.x);
                    float randZ = Random.Range(frontLeftAreaTransform.position.z, backRightAreaTransform.position.z);
                    Vector3 localPoint = new Vector3(randX, navMeshCenter.y, randZ);

                    Vector3 worldPoint = surfaceTransform.TransformPoint(localPoint);

                    if (NavMesh.SamplePosition(worldPoint, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
                    {
                        AI.SetDestination(hit.position);
                        moving = true;
                        StopCoroutine("ChangeWanderingTargetPosition");
                        StartCoroutine(ChangeWanderingTargetPosition(Random.Range(1, 4)));
                        return;
                    }
                }
            break;
            case States.Seeking:
                AI.isStopped = false;
                AI.SetDestination(seekPos);
                if(transform.position.x == seekPos.x && transform.position.z == seekPos.z)
                {
                    ChangeState(States.Wandering);
                    seekPos = Vector3.zero;
                }
            break;
            case States.Chasing:
                AI.isStopped = false;
                AI.SetDestination(target.position);
            break;

            case States.Stunned:
                AI.isStopped = true;
            break;
            case States.Attacking:
                AI.isStopped = true;
            break;
        }
    }

    IEnumerator ChangeIdleWanderingState(float timer)
    {
        yield return new WaitForSeconds(timer);
        if(actualState != States.Chasing && actualState != States.Stunned && actualState != States.Seeking && actualState != States.Attacking)
        {
            int newState = Random.Range(1, 3);
            if (newState == 1)
            {
                ChangeState(States.Idle);
            }
            else
            {
                ChangeState(States.Wandering);
            }
        }
        StartCoroutine(ChangeIdleWanderingState(Random.Range(1,4)));
    }

    IEnumerator ChangeWanderingTargetPosition(float timer)
    {
        yield return new WaitForSeconds(timer);
        moving = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (actualState != States.Stunned && !inCooldownAttack)
        {
            Attack(collision);
        }
    }

    private void Attack(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeState(States.Attacking);
            collision.gameObject.GetComponent<HealthScript>().Hurt(gameObject);
            inCooldownAttack = true;
            StartCoroutine(endAttack(2));
        }
    }

    IEnumerator endAttack(float timeToEnd)
    {
        yield return new WaitForSeconds(timeToEnd);
        if(actualState != States.Stunned)
        {
            ChangeState(States.Idle);
        }
        inCooldownAttack = false;
    }    

    public void reduceVision()
    {
        farVisionCone.transform.localScale = new Vector3(farVisionCone.transform.localScale.x / 2, farVisionCone.transform.localScale.y, farVisionCone.transform.localScale.z);
        nearVisionCone.transform.localScale = new Vector3(nearVisionCone.transform.localScale.x / 2, nearVisionCone.transform.localScale.y, nearVisionCone.transform.localScale.z);
        peripherealLeftVisionCone.SetActive(false);
        peripherealRightVisionCone.SetActive(false);
        // peripherealLeftVisionCone.transform.localScale = new Vector3(peripherealLeftVisionCone.transform.localScale.x / 2, peripherealLeftVisionCone.transform.localScale.y, peripherealLeftVisionCone.transform.localScale.z);
        // peripherealRightVisionCone.transform.localScale = new Vector3(peripherealRightVisionCone.transform.localScale.x / 2, peripherealRightVisionCone.transform.localScale.y, peripherealRightVisionCone.transform.localScale.z);
    }

    public void incrementVision()
    {
        farVisionCone.transform.localScale = new Vector3(farVisionCone.transform.localScale.x * 2, farVisionCone.transform.localScale.y, farVisionCone.transform.localScale.z);
        nearVisionCone.transform.localScale = new Vector3(nearVisionCone.transform.localScale.x * 2, nearVisionCone.transform.localScale.y, nearVisionCone.transform.localScale.z);
        peripherealLeftVisionCone.SetActive(true);
        peripherealRightVisionCone.SetActive(true);
        // peripherealLeftVisionCone.transform.localScale = new Vector3(peripherealLeftVisionCone.transform.localScale.x * 2, peripherealLeftVisionCone.transform.localScale.y, peripherealLeftVisionCone.transform.localScale.z);
        //peripherealRightVisionCone.transform.localScale = new Vector3(peripherealRightVisionCone.transform.localScale.x * 2, peripherealRightVisionCone.transform.localScale.y, peripherealRightVisionCone.transform.localScale.z);
    }

    public void stun(Transform lastPlayerPos)
    {
        ChangeState(States.Stunned);
        StopAllCoroutines();
        StartCoroutine(stopStun(8, lastPlayerPos));
    }

    IEnumerator stopStun(float time, Transform lastPlayerPos)
    {
        yield return new WaitForSeconds(time);
        ChangeState(States.Seeking);
        seekPos = lastPlayerPos.position;
        StartCoroutine(ChangeIdleWanderingState(Random.Range(1, 4)));
    }

}
