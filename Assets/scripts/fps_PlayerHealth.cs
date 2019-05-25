using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;
public class fps_PlayerHealth : MonoBehaviour
{

    public bool isDead;
    public float resetAfterDeathTime = 2;
    public AudioClip deathClip;
    public AudioClip damageClip;
    public float maxHp = 100;
    public float hp = 100;
    public float recoverSpeed = 1;
    public Slider HPSlider;
    private float timer = 0;
    private FadeInOut fader;
    private ColorCorrectionCurves colorCurves;

    void Start()
    {
        hp = maxHp;
        fader = GameObject.FindGameObjectWithTag(tags.fader).GetComponent<FadeInOut>();
        colorCurves = GameObject.FindGameObjectWithTag(tags.mainCamera).GetComponent<ColorCorrectionCurves>();
        BleedBehavior.BloodAmount = 0;
        HPSlider.value = HPSlider.maxValue = hp;
    }
     void Update()
    {
        if (!isDead)
        {
            hp += recoverSpeed * Time.deltaTime;
            if (hp > maxHp)
                hp = maxHp;
            HPSlider.value = hp;
        }
        if (hp < 0)
        {
            if (!isDead)
                PlayerDead();
            else
                LevelReset();

        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;
        AudioSource.PlayClipAtPoint(damageClip, transform.position);
        BleedBehavior.BloodAmount += Mathf.Clamp01(damage / hp);
        hp -= damage;
        HPSlider.value = hp;
    }
    public void DisableInput()//玩家通关或者死亡时，禁止输入
    {
        transform.Find("FP_Camera/Weapon_Camera").gameObject.SetActive(false);
        this.GetComponent<AudioSource>().enabled = false;
        this.GetComponent<fps_PlayerControl>().enabled = false;
        this.GetComponent<fps_FPInput>().enabled = false;
        if (GameObject.Find("Canvas") != null)
            GameObject.Find("Canvas").SetActive(false);
        colorCurves.gameObject.GetComponent<fps_FPCamera>().enabled = false;

    }

    public void PlayerDead()
    {
        Time.timeScale = 1;
        isDead = true;
        colorCurves.enabled = true;
        DisableInput();
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
    }

    public void LevelReset()
    {
        timer += Time.deltaTime;
        colorCurves.saturation -= (Time.deltaTime / 2);
        colorCurves.saturation = Mathf.Max(0, colorCurves.saturation);
        if (timer >= resetAfterDeathTime)
            fader.EndScene();
    }
}
