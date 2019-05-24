using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_EnemyHealth : MonoBehaviour
{

    public float hp = 100;
    private Animator anim;
    private HashIDs hash;
    private bool isDead = false;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(tags.gameController).GetComponent<HashIDs>();

    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp<=0 && !isDead)
        {
            isDead = true;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<fps_EnemyAnimation>().enabled = false;
            GetComponent<fps_EnemyAI>().enabled = false;
            GetComponent<fps_EnemyShoot>().enabled = false;
            GetComponent<fps_EnemySight>().enabled = false;
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            GetComponentInChildren<Light>().enabled = false;
            GetComponentInChildren<LineRenderer>().enabled = false;

            anim.SetBool(hash.playerInSightBool, false);
            anim.SetBool(hash.deadBool,true);

        }
    }
}
