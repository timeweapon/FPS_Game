using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class fps_FPCamera : MonoBehaviour
{
    public Vector2 mouseLookSensitivity = new Vector2(5, 5);//鼠标灵敏度
    public Vector2 rotationXLimit = new Vector2(87, -87);
    public Vector2 rotationYLimit = new Vector2(-360, 360);
    public Vector2 positionOffset = new Vector3(0,2,-0.2f);

    private Vector2 currentMouseLook = Vector2.zero;
    private float x_Angle = 0;
    private float y_Angle = 0;
    private fps_PlayerParameter parameter;
    private Transform m_Transform;
    void Start()
    {
        parameter = GameObject.FindGameObjectWithTag(tags.player).GetComponent<fps_PlayerParameter>();
        m_Transform = transform;
        m_Transform.localPosition = positionOffset;

    }
    private void Update()
    {
        UpdateInput();

    }
    private void LateUpdate()
    {
        Quaternion xQuaternion = Quaternion.AngleAxis(y_Angle, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(0, Vector3.left);
        m_Transform.parent.rotation = xQuaternion * yQuaternion;

        yQuaternion = Quaternion.AngleAxis(-x_Angle, Vector3.left);
        m_Transform.rotation = xQuaternion * yQuaternion;
    }
    private void UpdateInput()
    {
        if (parameter.inputSmoothLook == Vector2.zero)
            return;
        GetMouseLook();
        y_Angle += currentMouseLook.x;
        x_Angle += currentMouseLook.y;

        y_Angle = y_Angle < -360 ? y_Angle += 360 : y_Angle;
        y_Angle = y_Angle > 360 ? y_Angle -= 360 : y_Angle;
        y_Angle = Mathf.Clamp(y_Angle, rotationYLimit.x, rotationYLimit.y);

        x_Angle = x_Angle < -360 ? x_Angle += 360 : x_Angle;
        x_Angle = x_Angle > 360 ? x_Angle -= 360 : x_Angle;
        x_Angle = Mathf.Clamp(x_Angle, -rotationXLimit.x, -rotationXLimit.y);

    }
    private void GetMouseLook()
    {
        currentMouseLook.x = parameter.inputSmoothLook.x;
        currentMouseLook.y = parameter.inputSmoothLook.y;

        currentMouseLook.x *= mouseLookSensitivity.x;
        currentMouseLook.y *= mouseLookSensitivity.y;

        currentMouseLook.y *= -1;
    }

}
