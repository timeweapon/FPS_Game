using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashIDs : MonoBehaviour
{
    public int deadBool;
    public int speedFloat;
    public int playerInSightBool;
    public int shotFloat;
    public int aimWeightFloat;
    public int angularSpeedFloat;

    void Awake()
    {
        deadBool = Animator.StringToHash("Dead");
        speedFloat = Animator.StringToHash("Speed");
        playerInSightBool = Animator.StringToHash("PlayerInSight");
        shotFloat = Animator.StringToHash("Shot");
        aimWeightFloat = Animator.StringToHash("AimWeight");
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");

    }
}
