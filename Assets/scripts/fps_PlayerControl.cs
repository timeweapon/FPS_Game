using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PlayerState
{
    None,
    Idle,
    Walk,
    Crouch,
    Run,
}
//slow 应该和其它状态是同时进行的
public class fps_PlayerControl : MonoBehaviour
{
    private PlayerState state = PlayerState.None;
    public PlayerState State
    {
        get
        {
            if (running)
                state = PlayerState.Run;
            else if (walking)
                state = PlayerState.Walk;
            else if (crouching)
                state = PlayerState.Crouch;
            else state = PlayerState.Idle;
            return state;
        }
    }

    public Slider SlowSlider;
    //public TimeManager timeManager;
    public float sprintSpeed = 10.0f;
    public float sprintJumpSpeed = 8.0f;
    public float normalSpeed = 6.0f;
    public float normalJumpSpeed = 7.0f;
    public float crouchSpeed = 2.0f;
    public float crouchJumpSpeed = 5.0f;
    public float crouchDeltaHeight = 0.5f;

    public float skillAbility = 0.0f; // 变缓的程度
    public float skillStartTime = 0.0f; // 刚按下子弹时间的时间
    public float skillEndTime = 0.0f; // 技能结束时间
    public float skillColdTime = 2.0f; // 技能冷却时间
    public float skillContinueTime = 10.0f; // 技能持续时间
    public bool useSkill = false; // 用过技能否?

    public float gravity = 20.0f;
    public float cameraMoveSpeed = 8.0f;
    public AudioClip jumpAudio;

    private float speed;
    private float jumpSpeed;
    private Transform mainCamera;
    private float standardCamHeight;
    private float crouchingCamHeight;
    private bool grounded = false;
    private bool walking = false;
    private bool crouching = false;
    private bool stopCrouching = false;
    private bool running = false;
    private Vector3 normalControllerCenter = Vector3.zero;

    private float normalControllerHeight = 0.0f;
    private float timer = 0;
    private CharacterController controller;
    private AudioSource audioSource;
    private fps_PlayerParameter parameter;

    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        crouching = false;
        walking = false;
        running = false;
        useSkill = false;
        speed = normalSpeed;
        jumpSpeed = normalJumpSpeed;
        mainCamera = GameObject.FindGameObjectWithTag(tags.mainCamera).transform;
        standardCamHeight = mainCamera.localPosition.y;
        crouchingCamHeight = standardCamHeight - crouchDeltaHeight;
        audioSource = this.GetComponent<AudioSource>();
        controller = this.GetComponent<CharacterController>();
        parameter = this.GetComponent<fps_PlayerParameter>();
        normalControllerCenter = controller.center;
        normalControllerHeight = controller.height;
        SlowSlider.value = SlowSlider.maxValue = skillContinueTime;
    }
    private void FixedUpdate()
    {
        UpdateMove();
        AudioManagement();
        //UpdateFixedTime();
    }
    private void Update()
    {
        UpdateTime();
    }
    private void UpdateTime()
    { // TODO 做自动加回去的设计


        //if (skillStartTime <= 0.0001f
        //     && (Time.realtimeSinceStartup - skillEndTime > (skillColdTime) || !useSkill))
        //{
        //    SlowSlider.value = SlowSlider.maxValue;
        //}
        if (skillStartTime <= 0.0001f
             && (useSkill))
        {
            SlowSlider.value = Time.realtimeSinceStartup - skillEndTime;
        }
        if (parameter.inputTimeSlow && skillStartTime <= 0.0001f
             && (Time.realtimeSinceStartup - skillEndTime > (skillColdTime) || !useSkill)
            // 此处不知道为什么测试中的实际冷却时间是原来的3倍
            // 应该是别的地方有定义 coldtime 为15, 所以初始化无效, 改变量名以后则正常
            )
        {
            useSkill = true;
            skillStartTime = Time.realtimeSinceStartup;
            Time.timeScale = skillAbility;
            //audioSource.pitch *= skillAbility;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            //speed /= skillAbility; 
            //jumpSpeed /= skillAbility;
            cameraMoveSpeed /= skillAbility;
            SlowSlider.maxValue = skillContinueTime;
            SlowSlider.value = SlowSlider.maxValue;
            //Debug.Log("skillStartTime, 111111.. " + skillStartTime);
            //Debug.Log("cold?, 111111.. " + (Time.realtimeSinceStartup - skillEndTime)+ "   skillColdTime:  " + skillColdTime);
        }
        else
        {
            if (skillStartTime != 0 && Time.realtimeSinceStartup - skillStartTime >= skillContinueTime)
            {
                SlowSlider.value = 0;
                SlowSlider.maxValue = skillColdTime;
                Time.timeScale /= skillAbility;
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
                //audioSource.pitch /= skillAbility;
                //speed *= skillAbility;
                //jumpSpeed *= skillAbility;
                cameraMoveSpeed *= skillAbility;
                skillStartTime = 0.0f;
                skillEndTime = Time.realtimeSinceStartup;
                //Debug.Log("skillStartTime, 22222222222222222222.. " + skillStartTime);
                //Debug.Log("cold?, 22222222222222222222.. " + (Time.realtimeSinceStartup - skillEndTime));
            }
            else if (skillStartTime != 0)
            {
                float tmp = skillContinueTime - (Time.realtimeSinceStartup - skillStartTime);
                SlowSlider.value = tmp >= 0 ? tmp : 0;
            }
        }

    }
    private void UpdateMove()
    {
        if (grounded)
        {
            moveDirection = new Vector3(parameter.inputMoveVector.x, 0, parameter.inputMoveVector.y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (parameter.inputJump)
            {
                moveDirection.y = jumpSpeed;
                AudioSource.PlayClipAtPoint(jumpAudio, transform.position);
                CurrentSpeed();
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime);
        grounded = (flags & CollisionFlags.CollidedBelow) != 0;

        if (Mathf.Abs(moveDirection.x) > 0 && grounded || Mathf.Abs(moveDirection.z) > 0 && grounded)
        {
            if (parameter.inputSprint)
            {
                walking = false;
                running = true;
                crouching = false;
            }
            else if (parameter.inputCrouch)
            {
                walking = false;
                running = false;
                crouching = true;
            }
            else
            {
                walking = true;
                running = false;
                crouching = false;
            }
        }
        else
        {
            if (walking)
                walking = false;
            if (running)
                running = false;
            if (parameter.inputCrouch)
                crouching = true;
            else
                crouching = false;
        }

        if (crouching)
        {
            controller.height = normalControllerHeight - crouchDeltaHeight;
            controller.center = normalControllerCenter - new Vector3(0, crouchDeltaHeight / 2, 0);
        }
        else
        {
            controller.height = normalControllerHeight;
            controller.center = normalControllerCenter;
        }
        UpdateCrouch();
        CurrentSpeed();
    }
    private void CurrentSpeed()
    {
        switch (State)
        {

            case PlayerState.Idle:
                speed = normalSpeed;
                jumpSpeed = normalJumpSpeed;
                break;
            case PlayerState.Walk:
                speed = normalSpeed;
                jumpSpeed = normalJumpSpeed;
                break;
            case PlayerState.Crouch:
                speed = crouchSpeed;
                jumpSpeed = crouchJumpSpeed;
                break;
            case PlayerState.Run:
                speed = sprintSpeed;
                jumpSpeed = sprintJumpSpeed;
                break;
        }
    }
    private void AudioManagement()
    {
        if (state == PlayerState.Walk)
        {
            audioSource.pitch = 1.0f;
            //if (skillStartTime != 0) audioSource.pitch = skillAbility; else audioSource.pitch = 1.0f;
            if (!audioSource.isPlaying)
                audioSource.Play();

        }
        else if (state == PlayerState.Run)
        {
            audioSource.pitch = 1.3f;
            //if (skillStartTime != 0) audioSource.pitch = skillAbility; else audioSource.pitch = 1.3f;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
            audioSource.Stop();
    }

    private void UpdateCrouch()
    {
        if (crouching)
        {
            if (mainCamera.localPosition.y > crouchingCamHeight)
            {
                if (mainCamera.localPosition.y - (crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed) < crouchingCamHeight)
                    mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, crouchingCamHeight, mainCamera.localPosition.z);
                else
                    mainCamera.localPosition -= new Vector3(0, crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed, 0);
            }
            else
                mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, crouchingCamHeight, mainCamera.localPosition.z);

        }
        else
        {
            if (mainCamera.localPosition.y < standardCamHeight)
            {
                if (mainCamera.localPosition.y + (crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed) > standardCamHeight)
                    mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, standardCamHeight, mainCamera.localPosition.z);
                else
                    mainCamera.localPosition += new Vector3(0, crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed, 0);
            }
            else
                mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, standardCamHeight, mainCamera.localPosition.z);
        }
    }
}