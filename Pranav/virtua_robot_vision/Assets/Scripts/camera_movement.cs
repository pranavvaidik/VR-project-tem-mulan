using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
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
            x_rotation = transform.localRotation.eulerAngles.x;// + camera_speed * Time.deltaTime;
            y_rotation = transform.localRotation.eulerAngles.y + camera_speed * Time.deltaTime;// + Time.deltaTime;
            z_rotation = transform.localRotation.eulerAngles.z;// + Time.deltaTime;
            transform.rotation = Quaternion.Euler(x_rotation, y_rotation, z_rotation);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.forward * Time.deltaTime*camera_speed;//+= Vector3.down * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * Time.deltaTime * camera_speed;//Vector3.up * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x_rotation = transform.localRotation.eulerAngles.x;// + camera_speed * Time.deltaTime;
            y_rotation = transform.localRotation.eulerAngles.y - camera_speed * Time.deltaTime;// + Time.deltaTime;
            z_rotation = transform.localRotation.eulerAngles.z;// + Time.deltaTime;
            transform.rotation = Quaternion.Euler(x_rotation, y_rotation, z_rotation);
        }


        /*
        if (Input.GetKey(KeyCode.I))
        {

            x_rotation = transform.localRotation.eulerAngles.x;// + camera_speed * Time.deltaTime;
            y_rotation = transform.localRotation.eulerAngles.y + camera_speed * Time.deltaTime;// + Time.deltaTime;
            z_rotation = transform.localRotation.eulerAngles.z;// + Time.deltaTime;
            transform.rotation = Quaternion.Euler(x_rotation, y_rotation, z_rotation);
        }
        if (Input.GetKey(KeyCode.O))
        {
            x_rotation = transform.localRotation.eulerAngles.x ;
            y_rotation = transform.localRotation.eulerAngles.y - camera_speed * Time.deltaTime;// + Time.deltaTime;
            z_rotation = transform.localRotation.eulerAngles.z;// + Time.deltaTime;
            transform.rotation = Quaternion.Euler(x_rotation, y_rotation, z_rotation);
        }
        */
    }
}
