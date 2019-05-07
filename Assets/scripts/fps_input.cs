using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_input : MonoBehaviour
{
  public class fps_InputAxie
    {
        public KeyCode positive;
        public KeyCode negative;
    }
    public Dictionary<string, KeyCode> buttons = new Dictionary<string, KeyCode>();
    public Dictionary<string, fps_InputAxie> axis = new Dictionary<string, fps_InputAxie>();

    public List<string> unityAxis = new List<string>();
    void Start()
    {
        SetupDefaults();
    }

    private void SetupDefaults(string type = "")
    {
        if (type == "" || type == "buttons")
        {
            if (buttons.Count == 0)
            {
                AddButton("Fire", KeyCode.Mouse0);
                AddButton("Reload",KeyCode.R);
                AddButton("Jump",KeyCode.Space);
                AddButton("Crouch",KeyCode.C);
                AddButton("Sprint",KeyCode.LeftShift);
            }
        }

        if (type == "" || type == "Axis")
        {
            if (axis.Count == 0)
            {
                AddAxis("Horizontal", KeyCode.W, KeyCode.S);
                AddAxis("Vertical", KeyCode.A, KeyCode.D);
            }
        }
        if (type == "" || type == "UnityAxis")
        {
            if (unityAxis.Count == 0)
            {
                AddUnityAxis("Mouse X");
                AddUnityAxis("Mouse Y");
                AddUnityAxis("Horizontal");
                AddUnityAxis("Vertical");
            }
        }
    }
    private void AddButton(string n, KeyCode k)
    {
        if (buttons.ContainsKey(n))
            buttons[n] = k;
        else
            buttons.Add(n, k);
    }
    private void AddAxis(string n,KeyCode pk, KeyCode nk)
    {
        if (axis.ContainsKey(n))
        {
            axis[n] = new fps_InputAxie() { positive = nk, negative = pk };
        }
        else
            axis.Add(n, new fps_InputAxie() { positive = pk, negative = nk });
    }
    private void AddUnityAxis(string n)
    {
        if (!unityAxis.Contains(n))
            unityAxis.Add(n);
    }

    public bool GetButton(string button)
    {
        if (buttons.ContainsKey(button))
            return Input.GetKey(buttons[button]);
        return false;
    }
    public bool GetButtonDown(string button)
    {
        if (buttons.ContainsKey(button))
            return Input.GetKeyDown(buttons[button]);
        return false;
    }
    public float GetAxis(string axis)
    {
        if (this.unityAxis.Contains(axis))
            return Input.GetAxis(axis);
        else
            return 0;
    }

    public float GetAxisRaw(string axis)
    {
        if (this.axis.ContainsKey(axis))
        {
            float val = 0;
            if (Input.GetKey(this.axis[axis].positive))
                return 1;
            if (Input.GetKey(this.axis[axis].negative))
                return -1;
            return val;
        }
        else if (unityAxis.Contains(axis))
        {
            return Input.GetAxisRaw(axis);
        }
        else
        {
            return 0;
        }
    }
}
