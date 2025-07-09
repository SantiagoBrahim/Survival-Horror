using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Random = UnityEngine.Random;
using System.Collections;
using System;

public class AIDirector : MonoBehaviour
{

    private AIEnemy EnemyAI;
    private Transform playerTransform;


    private bool isMovingToPlayer;

    void Awake()
    {
        EnemyAI = gameObject.GetComponent<AIEnemy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        StartCoroutine(SeekLoop());
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingToPlayer)
        {
            if (EnemyAI.actualState != AIStates.States.Stunned)
            {
                float distanceToPlayer = Vector3.Distance(gameObject.transform.position, playerTransform.position);
                Debug.Log(distanceToPlayer);
                if (distanceToPlayer < 10)
                {
                    EnemyAI.ChangeState(AIStates.States.Wandering);
                    isMovingToPlayer = false;
                }
            }
        }
    }

    IEnumerator SeekLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(20, 60));

            if (EnemyAI.actualState != AIStates.States.Stunned &&
                EnemyAI.actualState != AIStates.States.Chasing &&
                EnemyAI.actualState != AIStates.States.Attacking)
            {
                isMovingToPlayer = true;
                EnemyAI.ChangeState(AIStates.States.Seeking);
                EnemyAI.seekPos = playerTransform.position;
            }
        }
    }
}
