using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_cam : MonoBehaviour
{

    public float x_rotation = 0.0f;
    public float y_rotation = 0.0f;
    public float z_rotation = 0.0f;
    public float camera_speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        float xRot = rot.x;
        float yRot = rot.y;
        float zRot = rot.z;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * Time.deltaTime;
        }

        

        if (Input.GetKey(KeyCode.I))
        {
            
            x_rotation = transform.localRotation.eulerAngles.x + camera_speed * Time.deltaTime;
            y_rotation = transform.localRotation.eulerAngles.y;// + Time.deltaTime;
            z_rotation = transform.localRotation.eulerAngles.z;// + Time.deltaTime;
            transform.rotation = Quaternion.Euler(x_rotation, y_rotation, z_rotation);
        }
        if (Input.GetKey(KeyCode.O))
        {
            x_rotation = transform.localRotation.eulerAngles.x - camera_speed * Time.deltaTime;
            y_rotation = transform.localRotation.eulerAngles.y;// + Time.deltaTime;
            z_rotation = transform.localRotation.eulerAngles.z;// + Time.deltaTime;
            transform.rotation = Quaternion.Euler(x_rotation, y_rotation, z_rotation);
        }
    }
}
