using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_KeyPickUp : MonoBehaviour
{
    public AudioClip keyGrab;
    public int keyId;

    private GameObject player;
    private fps_PlayerInventory playerInventory;
     void Start()
    {
        player = GameObject.FindGameObjectWithTag(tags.player);
        playerInventory = player.GetComponent<fps_PlayerInventory>();

    }
     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            AudioSource.PlayClipAtPoint(keyGrab, transform.position);
            playerInventory.AddKey(keyId);
            Destroy(this.gameObject);
        }
    }
}
