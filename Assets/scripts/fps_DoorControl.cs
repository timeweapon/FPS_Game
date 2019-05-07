using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_DoorControl : MonoBehaviour
{
    public int doorId;
    public Vector3 from;
    public Vector3 to;
    public float fadeSpeed = 5;
    public bool requireKey = false;
    public AudioClip doorSwitchClip;
    public AudioClip accessDeniedClip;

    private Transform door;
    private GameObject player;
    private AudioSource audioSources;
    private fps_PlayerInventory playerInventory;
    private int count;
    public int Count
    {
        get { return count; }
        set
        {
            if (count == 0 && value == 1 || count == 1 && value == 0) {
                audioSources.clip = doorSwitchClip;
                audioSources.Play();
            }
            count = value;
        }
    }
     void Start()
    {
        if (transform.childCount > 0)
            door = transform.GetChild(0);
        player = GameObject.FindGameObjectWithTag(tags.player);
        playerInventory = player.GetComponent<fps_PlayerInventory>();
        audioSources = this.GetComponent<AudioSource>();
        door.localPosition = from;
    }
     void Update()
    {
        if (Count > 0)
            door.localPosition = Vector3.Lerp(door.localPosition, to, fadeSpeed * Time.deltaTime);
        else
            door.localPosition = Vector3.Lerp(door.localPosition, from, fadeSpeed * Time.deltaTime);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            if (requireKey)
            {
                if (playerInventory.HasKey(doorId))
                    Count++;
                else
                {
                    audioSources.clip = accessDeniedClip;
                    audioSources.Play();

                }
            }
            else
                Count++;
        }
        else if (other.gameObject.tag == tags.enemy && other is CapsuleCollider)
            Count++;
    }
     void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player || (other.gameObject.tag == tags.enemy && other is CapsuleCollider))
            Count = Mathf.Max(0, Count - 1);
    }
}
