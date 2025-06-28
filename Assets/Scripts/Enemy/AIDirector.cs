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

    float timer;

    private bool isMovingToPlayer;

    void Awake()
    {
        EnemyAI = gameObject.GetComponent<AIEnemy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(20, 60);
        playerTransform = GameObject.FindWithTag("Player").transform;
        StartCoroutine(SeekPlayer(timer));
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
    
    IEnumerator SeekPlayer(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (EnemyAI.actualState != AIStates.States.Stunned)
        {
            isMovingToPlayer = true;
            EnemyAI.ChangeState(AIStates.States.Seeking);
            EnemyAI.seekPos = playerTransform.position;
            timer = Random.Range(20, 60);
        }
        StartCoroutine(SeekPlayer(timer));
    }
}
