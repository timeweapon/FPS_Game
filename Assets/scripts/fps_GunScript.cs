using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void PlayerShoot();

public class fps_GunScript : MonoBehaviour
{

    public static event PlayerShoot PlayerShootEvent;
    public float fireRate = 0.5f;
    public float damage = 40;
    public float reloadTime = 1.5f;
    public float flashRate = 0.02f;
    public AudioClip fireAudio;
    public AudioClip reloadAudio;
    public AudioClip damageAudio;
    public AudioClip dryFireAudio;
    public GameObject explosion;
    public int bulletCount = 60; // 弹匣子弹数量
    public int chargerBulletCount = 120; //弹夹子弹
    public Text bulletText;

    private string reloadAnim = "Reload";
    private string fireAnim = "Single_Shot";
    private string walkAnim = "Walk";
    private string runAnim = "Run";
    private string jumpAnim = "Jump";
    private string idleAnim = "Idle";

    private Animation anim;
    private float nextFireTime = 0.0f; // 射击间隔
    private MeshRenderer flash; // 闪光效果
    private int currentBullet;
    private int currentChargeBullet;
    private fps_PlayerParameter parameter;
    private fps_PlayerControl playerControl;

    private void Start()
    {
        parameter = GameObject.FindGameObjectWithTag(tags.player).GetComponent<fps_PlayerParameter>();
        playerControl = GameObject.FindGameObjectWithTag(tags.player).GetComponent<fps_PlayerControl>();
        anim = this.GetComponent<Animation>();
        flash = this.transform.Find("muzzle_flash").GetComponent<MeshRenderer>();
        flash.enabled = false;
        currentBullet = bulletCount;
        currentChargeBullet = chargerBulletCount;
        bulletText.text = currentBullet + "/" + currentChargeBullet;
    }

    private void Update()
    {
        if (parameter.inputReload && currentBullet < bulletCount)
            Reload();

        if (parameter.inputFire && !anim.IsPlaying(reloadAnim))
            Fire();
        else if (!anim.IsPlaying(reloadAnim))
            StateAnim(playerControl.State);
    }

    private void ReloadAnim()
    {
        anim.Stop(reloadAnim);
        anim[reloadAnim].speed = (anim[reloadAnim].clip.length / reloadTime);
        anim.Rewind(reloadAnim);
        anim.Play(reloadAnim);
    }

    private IEnumerator ReloadFinish()
    {
        yield return new WaitForSeconds(reloadTime);
        if (currentChargeBullet >= bulletCount - currentBullet)
        {
            currentChargeBullet -= (bulletCount - currentBullet);
            currentBullet = bulletCount;
        }
        else
        {
            currentBullet += currentChargeBullet;
            currentChargeBullet = 0;
        }

        bulletText.text = currentBullet + "/" + currentChargeBullet;
    }

    private void Reload()
    {
        if (!anim.IsPlaying(reloadAnim))
        {
            if (currentChargeBullet > 0)
                StartCoroutine(ReloadFinish());
            else
            {
                anim.Rewind(fireAnim);
                anim.Play(fireAnim);
                AudioSource.PlayClipAtPoint(dryFireAudio, transform.position);
                return;
            }
            AudioSource.PlayClipAtPoint(reloadAudio, transform.position);
            ReloadAnim();
        }
    }

    private IEnumerator Flash()
    {
        flash.enabled = true;
        yield return new WaitForSeconds(flashRate);
        flash.enabled = false;
    }

    private void Fire()
    {
        if (currentBullet == 0 && currentChargeBullet == 0)
            return;
        if (Time.time > nextFireTime)
        {
            if (currentBullet <= 0)
            {
                Reload();
                nextFireTime = Time.time + fireRate;
                return;
            }
            currentBullet--;
            bulletText.text = currentBullet + "/" + currentChargeBullet;
        }
        DamageEnemy();
        if (PlayerShootEvent != null)
            PlayerShootEvent();
        AudioSource.PlayClipAtPoint(fireAudio, transform.position);
        anim.Rewind(fireAnim);
        anim.Play(fireAnim);
        StartCoroutine(Flash());
    }

    private void DamageEnemy()
    {
      
        
    }

    private void PlayerStateAnim(string animName)
    {
        if (!anim.IsPlaying(animName))
        {
            anim.Rewind(animName);
            anim.Play(animName);
        }
    }

    private void StateAnim(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                PlayerStateAnim(idleAnim);
                break;
            case PlayerState.Walk:
                PlayerStateAnim(walkAnim);
                break;
            case PlayerState.Crouch:
                PlayerStateAnim(walkAnim);
                break;
            case PlayerState.Run:
                PlayerStateAnim(runAnim);
                break;
        }

    }

}
