using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetup
{

    public float speedDampTime = 0.1f;
    public float angularSpeedDampTime = 0.7f;
    public float angleResponseTime = 1f;

    private Animator anim;
    private HashIDs hash;

    public AnimatorSetup(Animator anim,HashIDs hash)
    {
        this.anim = anim;
        this.hash = hash;

    }
    
    public void Setup(float speed,float angle)
    {
        float AngularSpeed = angle / angleResponseTime;

        anim.SetFloat(hash.speedFloat,speed,speedDampTime,Time.deltaTime);
        anim.SetFloat(hash.angularSpeedFloat, AngularSpeed, angularSpeedDampTime, Time.deltaTime);


    }

}
