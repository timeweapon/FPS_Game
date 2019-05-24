using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_EnemyShoot : MonoBehaviour
{
    public float maximumDamage = 120;
    public float minimumDamage = 45;
    public AudioClip shotClip;
    public float flashIntensity = 3f;
    public float fadeSpeed = 10f;

    private Animator anim;
    private HashIDs hash;
    private LineRenderer laserShotLine;
    private Light laserShotLight;
    private SphereCollider col;
    private Transform player;
    private fps_PlayerHealth playerHealth;
    private bool shooting;
    private float scaleDamage;

    private void Start()
    {
        hash = GameObject.FindGameObjectWithTag(tags.gameController).GetComponent<HashIDs>();
        anim = this.GetComponent<Animator>();
        laserShotLine = this.GetComponentInChildren<LineRenderer>();
        laserShotLight = laserShotLine.gameObject.GetComponent<Light>();

        col = GetComponentInChildren<SphereCollider>();
        player = GameObject.FindGameObjectWithTag(tags.player).transform;
        playerHealth = player.GetComponent<fps_PlayerHealth>();

        laserShotLine.enabled=false;
        laserShotLight.intensity = 0;

        scaleDamage = maximumDamage - minimumDamage;

    }

    void Update()
    {
        //hash = GameObject.FindGameObjectWithTag(tags.gameController).GetComponent<HashIDs>();
        float shot = anim.GetFloat(hash.shotFloat);

        if (shot > 0.05f && !shooting)
            Shoot();
        if(shot < 0.05f)
        {
            shooting = false;
            laserShotLine.enabled = false;

        }

        laserShotLight.intensity = Mathf.Lerp(laserShotLight.intensity,0f,fadeSpeed*Time.deltaTime);
    }

    private void OnAnimatorIK(int layerIndex)//实现指向玩家的效果
    {
        float aimWeight = anim.GetFloat(hash.aimWeightFloat);//是否完成举枪动作
        anim.SetIKPosition(AvatarIKGoal.RightHand, player.position + Vector3.up * 1.5f);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand,aimWeight);

    }

    private void Shoot()
    {
        shooting = true;
        float fractionalDistance = ((col.radius - Vector3.Distance(transform.position, player.position)) / col.radius);
        float damage = scaleDamage * fractionalDistance + minimumDamage;

        playerHealth.TakeDamage(damage);
        ShotEffects();

    }

    private void ShotEffects()
    {
        laserShotLine.SetPosition(0, laserShotLine.transform.position);

        laserShotLine.SetPosition(1, player.position + Vector3.up * 1.5f);


        laserShotLine.enabled = true;

        laserShotLight.intensity = flashIntensity;

        AudioSource.PlayClipAtPoint(shotClip, laserShotLight.transform.position);

    }
}
