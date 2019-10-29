using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control : MonoBehaviour
{

    public float x_rotation = 0.0f;
    public float y_rotation = 0.0f;
    public float z_rotation = 0.0f;
    public float camera_speed = 10;

    public int resWidth = 400;
    public int resHeight = 400;

    public bool takePicture = false;

    public new Camera camera;//= //GetComponent<Camera>;

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot()
    {
        takePicture = true;
    }

    void LateUpdate()
    {
        takePicture |= Input.GetKeyDown("k");
        if (takePicture)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takePicture = false;
        }
    }

   

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
            y_rotation = transform.localRotation.eulerAngles.y + 2 * camera_speed * Time.deltaTime;// + Time.deltaTime;
            z_rotation = transform.localRotation.eulerAngles.z;// + Time.deltaTime;
            transform.rotation = Quaternion.Euler(x_rotation, y_rotation, z_rotation);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.forward * Time.deltaTime * camera_speed;//+= Vector3.down * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * Time.deltaTime * camera_speed;//Vector3.up * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x_rotation = transform.localRotation.eulerAngles.x;// + camera_speed * Time.deltaTime;
            y_rotation = transform.localRotation.eulerAngles.y - 2 * camera_speed * Time.deltaTime;// + Time.deltaTime;
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
