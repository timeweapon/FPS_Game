using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTracing : MonoBehaviour
{
    public GameObject testcamera;
    public GameObject bluecamera;
    public GameObject orangecamera;
    public GameObject bluedoor;
    public GameObject orangedoor;
    public Vector3 mid;
    public Vector3 test;
    public Camera be;
    public bool transportenable = true;
    // Start is called before the first frame update
    public float TwoPointDistance3D(Vector3 p1, Vector3 p2)
    {

        float i = Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x)
                            + (p1.y - p2.y) * (p1.y - p2.y)
                            + (p1.z - p2.z) * (p1.z - p2.z));

        return i;
    }
    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("BlueIn");
        //Debug.Log(orangecamera.transform.rotation);
        if (transportenable)
        {
            orangedoor.GetComponent<OrangeTracing>().transportenable = false;
            if (other.GetComponent<CharacterController>() != null)
            {
                other.GetComponent<CharacterController>().enabled = false;
                other.GetComponent<fps_FPInput>().enabled = false;
                testcamera.GetComponent<fps_FPCamera>().y_Angle += orangedoor.transform.eulerAngles.y - bluedoor.transform.eulerAngles.y + 180;
            }
            other.transform.root.position = orangedoor.transform.position;
            if (other.GetComponent<CharacterController>() != null)
            {
                other.GetComponent<CharacterController>().enabled = true;
                other.GetComponent<fps_FPInput>().enabled = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("BlueOut");
        transportenable = true;
    }
    void Start()
    {
        testcamera = GameObject.Find("FP_Camera");
        bluedoor = GameObject.Find("BlueDoor");
        orangedoor = GameObject.Find("OrangeDoor");
        bluecamera = GameObject.Find("BlueCamera");
        orangecamera = GameObject.Find("OrangeCamera");
        be = orangecamera.GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        var cpos = testcamera.transform.position;
        var mt = bluedoor.transform.worldToLocalMatrix;
        mt = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(180, Vector3.up), Vector3.one) * mt;
        orangecamera.transform.localPosition = mt.MultiplyPoint(cpos);
        mid = orangecamera.transform.localPosition;
        mid[1] = -mid[1];
        mid[2] = -mid[2];
        orangecamera.transform.localPosition = mid;
        orangecamera.transform.LookAt(orangedoor.transform.position);
        be.nearClipPlane = TwoPointDistance3D(be.transform.position, orangedoor.transform.position);
        const float renderHeight = 4f;
        be.fieldOfView = 2 * Mathf.Atan(renderHeight / 2 / be.nearClipPlane) * Mathf.Rad2Deg;
    }
}
