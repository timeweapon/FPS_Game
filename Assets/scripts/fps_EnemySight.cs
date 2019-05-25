using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class fps_EnemySight : MonoBehaviour
{
    public float fieldOfViewAngle = 110f;//
    public bool PlayerInSight;
    public Vector3 playerPosition;
    public Vector3 resetPosition = Vector3.zero;

    private NavMeshAgent nav;
    private SphereCollider col;
    private Animator anim;
    private GameObject player;
    private fps_PlayerHealth playerHealth;
    private HashIDs hash;
    private fps_PlayerControl playerControl;

    void Start()
    {
        hash = GameObject.FindGameObjectWithTag(tags.gameController).GetComponent<HashIDs>();
        nav = this.GetComponent<NavMeshAgent>();
        col = GetComponentInChildren<SphereCollider>();
        anim = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(tags.player);
        playerHealth = player.GetComponent<fps_PlayerHealth>();
        playerControl = player.GetComponent<fps_PlayerControl>();

        fps_GunScript.PlayerShootEvent += ListenPlayer;

    }

    void Update()
    {
        //hash = GameObject.FindGameObjectWithTag(tags.gameController).GetComponent<HashIDs>();
        if (playerHealth.hp > 0)
            anim.SetBool(hash.playerInSightBool, PlayerInSight);
        else
            anim.SetBool(hash.playerInSightBool, false);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player)
        {
            PlayerInSight = false;
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle < fieldOfViewAngle * 0.5F)//射线判断障碍物
            {
                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out RaycastHit hit, col.radius)) ;
                {
                    if(hit.collider.gameObject==player)
                    {
                        PlayerInSight = true;
                        playerPosition = player.transform.position;

                    }

                }

            }
            if (playerControl.State == PlayerState.Walk || playerControl.State == PlayerState.Run) ;
            {
                ListenPlayer();

            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject==player)
        {
            PlayerInSight = false;
        }
    }

    private void ListenPlayer()//侦听=玩家距离
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= col.radius) ;
        playerPosition = player.transform.position;



    }
    void OnDestroy()//死亡取消
    {
        fps_GunScript.PlayerShootEvent -= ListenPlayer;
    }

}
