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
    private Vector3 originalFarVisionScale;
    private Vector3 originalNearVisionScale;

    // Ataque
    private bool inCooldownAttack;


    private Coroutine idleWanderCoroutine;

    private Coroutine wanderingCoroutine;



    private void Awake()
    {
        AI = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        originalFarVisionScale = farVisionCone.transform.localScale;
        originalNearVisionScale = nearVisionCone.transform.localScale;
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
                    float randX = Random.Range(Mathf.Min(frontLeftAreaTransform.position.x, backRightAreaTransform.position.x),
                                               Mathf.Max(frontLeftAreaTransform.position.x, backRightAreaTransform.position.x));

                    float randZ = Random.Range(Mathf.Min(frontLeftAreaTransform.position.z, backRightAreaTransform.position.z),
                                               Mathf.Max(frontLeftAreaTransform.position.z, backRightAreaTransform.position.z));

                    Vector3 localPoint = new Vector3(randX, navMeshCenter.y, randZ);

                    Vector3 worldPoint = surfaceTransform.TransformPoint(localPoint);

                    if (NavMesh.SamplePosition(worldPoint, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
                    {
                        AI.SetDestination(hit.position);
                        moving = true;
                        if (wanderingCoroutine != null) StopCoroutine(wanderingCoroutine);
                        wanderingCoroutine = StartCoroutine(ChangeWanderingTargetPosition());
                        return;
                    }
                }
            break;
            case States.Seeking:
                AI.isStopped = false;
                AI.SetDestination(seekPos);
                Debug.Log(Vector3.Distance(transform.position, seekPos));
                if(Vector3.Distance(transform.position, seekPos) < 5f)
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
            ChangeState(newState == 1 ? States.Idle : States.Wandering);
            idleWanderCoroutine = StartCoroutine(ChangeIdleWanderingState(Random.Range(1, 4)));
        }
    }

    private void StopIdleWanderRoutine()
    {
        if (idleWanderCoroutine != null)
        {
            StopCoroutine(idleWanderCoroutine);
            idleWanderCoroutine = null;
        }
    }

    IEnumerator ChangeWanderingTargetPosition()
    {
        yield return new WaitForSeconds(Random.Range(1, 4));
        moving = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (actualState != States.Stunned && !inCooldownAttack)
        {
            Attack(collision.gameObject);
        }
    }

    private void Attack(GameObject collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChangeState(States.Attacking);
            collision.GetComponent<HealthScript>().Hurt(gameObject);
            inCooldownAttack = true;
            StartCoroutine(endAttack(2));
        }
    }

    IEnumerator endAttack(float timeToEnd)
    {
        yield return new WaitForSeconds(timeToEnd);
        if(actualState != States.Stunned)
        {
            actualState = States.Wandering;
        }
        inCooldownAttack = false;
    }    

    public void reduceVision()
    {
        farVisionCone.transform.localScale = originalFarVisionScale / 2;
        nearVisionCone.transform.localScale = originalNearVisionScale / 2;
        peripherealLeftVisionCone.SetActive(false);
        peripherealRightVisionCone.SetActive(false);
    }

    public void incrementVision()
    {
        farVisionCone.transform.localScale = originalFarVisionScale;
        nearVisionCone.transform.localScale = originalNearVisionScale;
        peripherealLeftVisionCone.SetActive(true);
        peripherealRightVisionCone.SetActive(true);
    }

    public void stun(Transform lastPlayerPos)
    {
        ChangeState(States.Stunned);
        StopAllCoroutines();
        StartCoroutine(stopStun(10, lastPlayerPos));
    }

    IEnumerator stopStun(float time, Transform lastPlayerPos)
    {
        yield return new WaitForSeconds(time);
        actualState = States.Seeking;
        seekPos = lastPlayerPos.position;
    }

}
