using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_PlayerInventory : MonoBehaviour
{
    private List<int> keysArr;
    void Start()
    {
        keysArr = new List<int>(); 
    }
    public void AddKey(int keyId)
    {
        if (!keysArr.Contains(keyId))
            keysArr.Add(keyId);

    }
    public bool HasKey(int doorId)
    {
        if (keysArr.Contains(doorId))
            return true;
        return false;
    }
}
