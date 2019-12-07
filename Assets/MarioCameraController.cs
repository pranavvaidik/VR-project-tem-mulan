using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioCameraController : MonoBehaviour
{
    public GameObject robot;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - robot.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = robot.transform.position + offset;
        transform.rotation = robot.transform.rotation;

        Debug.Log("Camera Rotation is " + transform.rotation.eulerAngles + " Robot's Rotation is " + robot.transform.rotation.eulerAngles);

    }
}
