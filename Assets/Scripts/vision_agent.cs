using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class vision_agent : Agent
{
    new Camera camera;

    public float x_rotation = 0.0f;
    public float y_rotation = 0.0f;
    public float z_rotation = 0.0f;
    public float camera_speed = 10;

    public int resWidth = 0;
    public int resHeight = 0;

    public bool takePicture = false;

    public float centerX;
    public float centerY;
    public float radius;

    int num_colors_left;


    public string text_Action = "Not yet assigned";

    //public new Camera camera;//= //GetComponent<Camera>;

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
        camera = GetComponent<Camera>();

        resWidth = camera.pixelWidth;
        resHeight = camera.pixelHeight;

        Vector3 rotation = transform.localRotation.eulerAngles;
        float xRot = rotation.x;
        float yRot = rotation.y;
        float zRot = rotation.z;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        centerY = camera.pixelHeight - vectorAction[0];
        //centerY = 1.0f - vectorAction[0];
        centerX = vectorAction[1];
        radius = vectorAction[2];

        if (vectorAction[3] > 0)
        {
            RequestDecision();
        }

        string color;

        if (textAction.Equals("Y"))
        {
            color = "Yellow";
        }
        else if (textAction.Equals("O"))
        {
            color = "Orange";
        }
        else if (textAction.Equals("B"))
        {
            color = "Blue";
        }
        else if(textAction.Equals("G"))
        {
            color = "Green";
        }
        else if(textAction.Equals("R"))
        {
            color = "Red";
        }
        else if(textAction.Equals("P"))
        {
            color = "Purple";
        }
        else
        {
            color = "Unassigned";
        }







        Debug.Log("Pixel width :" + camera.pixelWidth + " Pixel height : " + camera.pixelHeight);
        Debug.Log("Vector action is: " + vectorAction[0].ToString());

        Debug.Log(textAction);

        Vector3 objCenter = new Vector3(centerX, centerY, 0);

        Ray ray = camera.ScreenPointToRay(objCenter);
        //Ray ray = camera.ViewportPointToRay(objCenter);

        string test;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform target = hit.transform;


            Vector3 objLoc = camera.WorldToScreenPoint(target.position);
            //Vector3 objLoc = camera.WorldToViewportPoint(target.position);
            objLoc.z = 0.0f ;

            Debug.Log("Raycast hit the object with name: " +target.name);


            //target.tag = "Button";

            Debug.LogFormat("objLog is {0}, objCenter is {1} and the distance is {2}, radius is {3}", objLoc, objCenter, Vector3.Distance(objLoc, objCenter), radius);
            
            if (Vector3.Distance(objLoc, objCenter) < radius)
            {
                Debug.Log("an object is found, " + target.position);



                if (target.CompareTag("Untagged"))
                {
                    test = "Button";
                    test = test + textAction;
                    text_Action = color;

                    

                    if (string.Equals(test,textAction))
                    {
                        Debug.LogFormat("both are same, then what is the issue?");
                    }
                    else
                    {
                        //Debug.
                        Debug.LogFormat("Both are different. Bigger issue" + test);
                    }

                    Debug.LogFormat("current tag is: {0}, while text action is {1}", text_Action, textAction);

                    
                    target.tag = color;
                }
            }
            
        }


        // Make sure that object object center is close to raycast hit point for double-checking the objects detected

        //change tag of objects to detected labels

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

        if (Input.GetKeyDown("s"))
        {
            ScanVisibleArea();
        }

    }

    public void ScanVisibleArea()
    {
        // Make sure robot is not moving


        // Call the request action from python API for all the object screen locations

        // for each object location, use raycast to find the specific objects found
        RequestDecision();

    }
}
