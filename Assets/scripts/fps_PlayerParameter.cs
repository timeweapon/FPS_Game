using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class fps_PlayerParameter : MonoBehaviour
{
    [HideInInspector]
    public Vector2 inputSmoothLook;//鼠标速度
    [HideInInspector]
    public Vector2 inputMoveVector;//按键
    [HideInInspector]
    public bool inputCrouch;
    [HideInInspector]
    public bool inputJump;
    [HideInInspector]
    public bool inputSprint;
    [HideInInspector]
    public bool inputFire;
    [HideInInspector]
    public bool inputReload;

}
