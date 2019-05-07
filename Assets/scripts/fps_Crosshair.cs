using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_Crosshair : MonoBehaviour
{
    public float length;
    public float width;
    public float distance;
    public Texture2D crosshariTexture;
    private GUIStyle lineStyle;
    private Texture tex;

     void Start()
    {
        lineStyle = new GUIStyle();
        lineStyle.normal.background = crosshariTexture;

    }

    private void OnGUI()
    {
        GUI.Box(new Rect((Screen.width - distance) / 2 - length, (Screen.height - width) / 2, length, width),tex, lineStyle);

        GUI.Box(new Rect((Screen.width + distance) / 2 , (Screen.height - width) / 2, length, width), tex, lineStyle);
        GUI.Box(new Rect((Screen.width - width) / 2, (Screen.height - distance) / 2-length, width, length), tex, lineStyle);
        GUI.Box(new Rect((Screen.width - width) / 2, (Screen.height + distance) / 2 , width, length), tex, lineStyle);
    }

}
