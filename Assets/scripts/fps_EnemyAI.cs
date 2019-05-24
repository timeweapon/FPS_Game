using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class fps_EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float chaseWaitTime = 5f;
    public float patrolWaitTime = 1f;
    public Transform[] patrolWayPoint;

    private fps_EnemySight enemySight;
    private NavMeshAgent nav;
    private Transform player;
    private fps_PlayerHealth playerHealth;
    private float chaseTimer;
    private float patrolTimer;
    private int wayPointIndex;

    void Start()
    {
        enemySight = this.GetComponent<fps_EnemySight>();
        nav = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(tags.player).transform;
        playerHealth = player.GetComponent<fps_PlayerHealth>();
    }

    void Update()
    {
        if (enemySight.PlayerInSight && playerHealth.hp > 0f)
            Shooting();
        else if (enemySight.playerPosition != enemySight.resetPosition && playerHealth.hp > 0f)//追击
            Chasing();
        else
            Patrolling();
    }

    private void Shooting()
    {
        nav.SetDestination(transform.position);
    }

    private void Chasing()
    {
        Vector3 sightingDeltaPos = enemySight.playerPosition - transform.position;

        if (sightingDeltaPos.sqrMagnitude > 4f)
            nav.destination = enemySight.playerPosition;

        nav.speed = chaseSpeed;

        if (nav.remainingDistance < nav.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;

            if (chaseTimer >= chaseWaitTime)
            {
                enemySight.playerPosition = enemySight.resetPosition;
                chaseTimer = 0f;
            }
        }
        else
            chaseTimer = 0;
    }

    private void Patrolling()
    {
        nav.speed = patrolSpeed;

        if (nav.destination == enemySight.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                if (wayPointIndex == patrolWayPoint.Length - 1)
                    wayPointIndex = 0;
                else
                    wayPointIndex++;

                patrolTimer = 0;
            }
        }
        else
            patrolTimer = 0;

        nav.destination = patrolWayPoint[wayPointIndex].position;

    }


}
