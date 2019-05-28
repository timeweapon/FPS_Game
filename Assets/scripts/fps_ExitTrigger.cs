using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fps_ExitTrigger : MonoBehaviour
{
    public float timeToInactivePlayer = 2.0f;
    public float timeToRestart = 5.0f;

    private GameObject player;
    private bool playerInExit;
    private float timer;
    private FadeInOut fader;

     void Start()
    {
        fader = GameObject.FindGameObjectWithTag(tags.fader).GetComponent<FadeInOut>();
        player=GameObject.FindGameObjectWithTag(tags.player);
    }
      void Update()
    {
        if (playerInExit)
            InExitActivation();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInExit = true;
           

        }
    }

      void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInExit = false;
            timer = 0;
        }
    }
    private void InExitActivation()
    {
        timer += Time.deltaTime;
        if (timer >= timeToInactivePlayer)
        {
            player.GetComponent<fps_PlayerHealth>().DisableInput();

        }
        if (timer >= timeToRestart)
            SceneManager.LoadScene("Start");
    }
}
