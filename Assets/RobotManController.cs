using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MLAgents;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class RobotManController : Agent
{
    //[SerializeField]
    //Transform _destination;
    [SerializeField]
    private Transform ExitDoor1to2;

    [SerializeField]
    private Transform ExitDoor2to3;

    Animator door1to2;
    Animator door2to3;

    [SerializeField]
    Transform Room2Center;
    [SerializeField]
    Transform Room3Center;

    [SerializeField]
    Transform ButtonStructureRoom1;

    [SerializeField]
    Transform ButtonStructureRoom2;

    [SerializeField]
    Transform ButtonStructureRoom3;

    [SerializeField]
    Transform RayCastObject;

    [SerializeField]
    Transform WatsonObject;

    GameObject[] GameButtons;

    NavMeshAgent _navMeshAgent;
    Animator anim;

    List<Vector3> TravelPositions = new List<Vector3>();
    int currentDestNum = 0;
    string[] colors;
    string textfromNLP;

    private bool pleaseidle = true;

    public new Camera camera;
    public string text_Action = "Not yet assigned";
    public bool takePicture = false;
    public int resWidth = 0;
    public int resHeight = 0;

    public float centerX;
    public float centerY;
    public float radius;

    [Tooltip("Text object where the parsed command will be held.")]
    public TextMesh CommandField;

    //vision_agent vision_Agent = new vision_agent();

    private float timer = 0.0f;
    public float waitingTime = 8.0f;

    private int in_room = 1;
    private int cam_pos = 1;
    private int room_1_scanned = 0;
    private int room_2_scanned = 0;
    private int room_3_scanned = 0;
    private int can_jump = 1;

    private Vector3 cam_loc_1 = new Vector3(-15.4f, 1.647289f, -3);
    private Vector3 cam_loc_2 = new Vector3(-15.4f, 1.647289f, 3);
    private Vector3 cam_loc_3 = new Vector3(-15.4f, 1.647289f, 13);
    private Vector3 cam_loc_4 = new Vector3(-15.4f, 1.647289f, 19);
    private Vector3 cam_loc_5 = new Vector3(-15.4f, 1.647289f, 29);
    private Vector3 cam_loc_6 = new Vector3(-15.4f, 1.647289f, 35);

    private bool isdoorto2 = true;
    private bool isdoorto3 = true;

    void Start()
    {

        textfromNLP = CommandField.text;

        door1to2 = ExitDoor1to2.GetComponent<Animator>();
        door2to3 = ExitDoor2to3.GetComponent<Animator>();

        resWidth = camera.pixelWidth;
        resHeight = camera.pixelHeight;

        anim = GetComponent<Animator>();
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            Debug.Log("Call Pranav function first time");
            //ScanVisibleArea();
            SetDestinations(textfromNLP);
        }

    }

    private void ScanToTag()
    {
        Debug.Log("Reached here");

        ScanVisibleArea();
        Debug.Log("Scan Ends");

    }

    IEnumerator AddingDelay()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("It adds delay here");
    }

    IEnumerator JumpCamera()
    {
        if (in_room == 1 && room_1_scanned == 0)
        {
            if (cam_pos == 1)
            {
                ScanVisibleArea(); //scan
                cam_pos = 2;
                camera.GetComponent<Transform>().position = cam_loc_2;//move to pos 2
            }
            else if (cam_pos == 2)
            {
                ScanVisibleArea(); //scan
                cam_pos = 1;
                camera.GetComponent<Transform>().position = cam_loc_1;//move to pos 1
            }
                
        }
        else if (in_room == 2 && room_2_scanned == 0)
        {
            if (cam_pos == 3)
            {
                ScanVisibleArea(); //scan
                cam_pos = 4;
                camera.GetComponent<Transform>().position = cam_loc_4;//move to pos 4
            }
            else if (cam_pos == 4)
            {
                ScanVisibleArea(); //scan
                cam_pos = 3;
                camera.GetComponent<Transform>().position = cam_loc_3;//move to pos 3
            }
        }
        else if (in_room == 3 && room_3_scanned == 0)
        {
            if (cam_pos == 5)
            {
                ScanVisibleArea(); //scan
                cam_pos = 6;
                camera.GetComponent<Transform>().position = cam_loc_6;//move to pos 6
            }
            else if (cam_pos == 6)
            {
                ScanVisibleArea(); //scan
                cam_pos = 5;
                camera.GetComponent<Transform>().position = cam_loc_5;//move to pos 5
            } 
        }
        yield return new WaitForSeconds(0.1F); //2 second stall
        can_jump = 1;
        Debug.Log("Can change cameras again");
    }

    private void SetDestinations(string data)
    {
        Debug.Log("CommandField text from Andrew are " + data);

        char[] separator = { ' ', ',' };
        colors = data.Split(separator);

        foreach (string color in colors)
        {
            Debug.Log("Colors Split Test " + color);
        }

        foreach (string color in colors)
        {

            GameButtons = GameObject.FindGameObjectsWithTag(color);

            if (GameButtons.Length == 0)
            {
                Debug.Log("Calling Pranav's Function second time");
                //ScanToTag();
                GameButtons = GameObject.FindGameObjectsWithTag(color);
            }

            if (GameButtons.Length > 0)
            {
                Debug.Log("Scanned and Tagged!");
                foreach (GameObject GameButton in GameButtons)
                {
                    Debug.Log("Tagged Object Name - " + GameButton.name);
                    Debug.Log("Tagged as - " + GameButton.tag);
                }

                foreach (GameObject GameButton in GameButtons)
                {
                    Vector3 targetVector = GameButton.transform.position;
                    targetVector[0] = targetVector[0] + 0.75f;   //Adding offset based on Arms length
                    targetVector[1] = 0.0f;  //Robot moves only in x and z direction
                    targetVector[2] = targetVector[2] - 0.3f;   //Adding offset based on Arms length
                    TravelPositions.Add(targetVector);
                    //GoToNextPoint();
                }
            }
            else
            {
                Debug.Log("Game Object not found with color " + color);
            }
            Debug.Log("End Color Iteration");
        }
        Debug.Log("Travel Positions: " + TravelPositions.Count);
        GoToNextPoint();

        if (TravelPositions.Count != 0)
        {
            pleaseidle = false;
        }
    }


    private void Update()
    {

        Debug.Log("|Current Destination Number =" + currentDestNum);
        if (can_jump == 1)
        {
            can_jump = 0; //won't be able to jump until coroutine finishes
            StartCoroutine(JumpCamera());

            if (ButtonStructureRoom1.GetChild(1).GetChild(0).GetChild(0).tag == "yellow" &&
                ButtonStructureRoom1.GetChild(1).GetChild(1).GetChild(0).tag == "green" &&
                ButtonStructureRoom1.GetChild(1).GetChild(2).GetChild(0).tag == "red" &&
                ButtonStructureRoom1.GetChild(1).GetChild(3).GetChild(0).tag == "blue" &&
                ButtonStructureRoom1.GetChild(1).GetChild(4).GetChild(0).tag == "cyan" &&
                ButtonStructureRoom1.GetChild(1).GetChild(5).GetChild(0).tag == "purple")
            {
                room_1_scanned = 1;
            }

            if (ButtonStructureRoom2.GetChild(1).GetChild(0).GetChild(0).tag == "yellow" &&
                ButtonStructureRoom2.GetChild(1).GetChild(1).GetChild(0).tag == "green" &&
                ButtonStructureRoom2.GetChild(1).GetChild(2).GetChild(0).tag == "red" &&
                ButtonStructureRoom2.GetChild(1).GetChild(3).GetChild(0).tag == "blue" &&
                ButtonStructureRoom2.GetChild(1).GetChild(4).GetChild(0).tag == "cyan" &&
                ButtonStructureRoom2.GetChild(1).GetChild(5).GetChild(0).tag == "purple")
            {
                room_2_scanned = 1;
            }

            if (ButtonStructureRoom3.GetChild(1).GetChild(0).GetChild(0).tag == "yellow" &&
                ButtonStructureRoom3.GetChild(1).GetChild(1).GetChild(0).tag == "green" &&
                ButtonStructureRoom3.GetChild(1).GetChild(2).GetChild(0).tag == "red" &&
                ButtonStructureRoom3.GetChild(1).GetChild(3).GetChild(0).tag == "blue" &&
                ButtonStructureRoom3.GetChild(1).GetChild(4).GetChild(0).tag == "cyan" &&
                ButtonStructureRoom3.GetChild(1).GetChild(5).GetChild(0).tag == "purple")
            {
                room_3_scanned = 1;
            }
        }


        string textcheck = CommandField.text;
        if (textcheck != textfromNLP)
        {
            Debug.Log("TextMesh Changed");
            textfromNLP = textcheck;
            currentDestNum = 0;
            TravelPositions.Clear();
            SetDestinations(textfromNLP);
        }


        if (Input.GetKeyDown("s"))
        {
            Debug.Log("key was detected");
            ScanVisibleArea();
            //SetDestinations();
        }

        if (pleaseidle == true)
        {
            Debug.Log("Robot Idle");
            AgentIdle();
            //ScanToTag();
            //ScanVisibleArea();
            //camera.transform.rotation = Quaternion.Euler(0, 300, 0);
            //ScanVisibleArea();
        }
        if ( _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            Debug.Log("Robot walk");
            AgentWalk();
        }
        else if (!_navMeshAgent.pathPending && (_navMeshAgent.remainingDistance <= _navMeshAgent.remainingDistance) && (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f))
        {
            Debug.Log("pathPending status =" + _navMeshAgent.pathPending);
            //ispressing = true;

            //Debug.Log("Robot Orientation is " + _navMeshAgent.transform.rotation.eulerAngles);
            Vector3 robotangles = _navMeshAgent.transform.rotation.eulerAngles;
            if (robotangles[1] < 280)
            {
                Debug.Log("Turning Right");
                //AgentIdle();
                AgentRightTurn();
                //robotangles += new Vector3(x, y, z) * Time.deltaTime * rotationSpeed;
                transform.rotation = Quaternion.Euler(0, 280, 0); ;
                Debug.Log("RobotAngle changes to " + robotangles);

            }
            else if (robotangles[1] > 280)
            {
                Debug.Log("Turning Left");
                //AgentIdle();
                AgentLeftTurn();
                //robotangles += new Vector3(x, y, z) * Time.deltaTime * rotationSpeed;
                transform.rotation = Quaternion.Euler(0, 280, 0); ;
                Debug.Log("RobotAngle changes to " + robotangles);

            }
            else
            {
                AgentPress();
            }
            //AgentPress();
            
            // This is where OutofRange error comes from. Instead of idling, press function keeps getting called and GoToNextPoint() kepps getting called every 5 sec.
            timer += Time.deltaTime;
            if (timer > waitingTime)
            {
                timer = 0.0f;
                GoToNextPoint();
                Debug.Log("I'm here....");
            }
            
        }

        if (GetComponent<Transform>().GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetComponent<collisiondetection>().isbuttonpressed == true)
        {
            GetComponent<Transform>().GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetComponent<collisiondetection>().isbuttonpressed = false;
            currentDestNum += 1;
        }

        if (door1to2.GetBool("open") == true && isdoorto2 == true)
        {
            TravelPositions.Clear();
            currentDestNum = 0;
            pleaseidle = true;
            _navMeshAgent.SetDestination(Room2Center.position);
            isdoorto2 = false; //This flag for the function to be called just once when the door opens. Otherwise it gets called every update that make robot unmovable in room 2
            
            if (in_room == 1)
            {
                in_room = 2;
                cam_pos = 3;
                RayCastObject.GetComponent<LightBeamSelect>().Should_Be_Active = false;//disable raycasting
                WatsonObject.GetComponent<WatsonKeywordExtractor>().Should_Be_Active = true;//enable Watson
            }
            
        }

        if (door2to3.GetBool("open") == true && isdoorto3 == true)
        {
            TravelPositions.Clear();
            currentDestNum = 0;
            pleaseidle = true;
            _navMeshAgent.SetDestination(Room3Center.position);
            isdoorto3 = false;
            
            if (in_room == 2)
            {
                in_room = 3;
                cam_pos = 5;
                RayCastObject.GetComponent<LightBeamSelect>().Should_Be_Active = true;//enable raycasting
                WatsonObject.GetComponent<WatsonKeywordExtractor>().Should_Be_Active = false;//disable Watson
            }
            
        }
    }

    private void GoToNextPoint()
    {
        //Check if there are points to walk to 
        Debug.Log("GoToNextPoint called");
        if (TravelPositions.Count == 0)
            Debug.Log("No TravelPositions");

        // Set the agent to go to the currently selected destination.
        _navMeshAgent.SetDestination(TravelPositions[currentDestNum]);

        // Choose the next point in the array as the destination
        //currentDestNum += 1;
    }

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

    // Update is called once per frame
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
            color = "yellow";
        }
        else if (textAction.Equals("C"))
        {
            color = "cyan";
        }
        else if (textAction.Equals("B"))
        {
            color = "blue";
        }
        else if (textAction.Equals("G"))
        {
            color = "green";
        }
        else if (textAction.Equals("R"))
        {
            color = "red";
        }
        else if (textAction.Equals("P"))
        {
            color = "purple";
        }
        else
        {
            color = "Unassigned";
        }

        //Debug.Log("Pixel width :" + camera.pixelWidth + " Pixel height : " + camera.pixelHeight);
        //Debug.Log("Vector action is: " + vectorAction[0].ToString());

        //Debug.Log(textAction);

        Vector3 objCenter = new Vector3(centerX, centerY, 0);

        Ray ray = camera.ScreenPointToRay(objCenter);
        //Ray ray = camera.ViewportPointToRay(objCenter);

        string test;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform target = hit.transform;


            Vector3 objLoc = camera.WorldToScreenPoint(target.position);
            //Vector3 objLoc = camera.WorldToViewportPoint(target.position);
            objLoc.z = 0.0f;

            //Debug.Log("Raycast hit the object with name: " + target.name);


            //target.tag = "Button";

            //Debug.LogFormat("objLog is {0}, objCenter is {1} and the distance is {2}, radius is {3}", objLoc, objCenter, Vector3.Distance(objLoc, objCenter), radius);

            if (Vector3.Distance(objLoc, objCenter) < radius)
            {
                Debug.Log("an object is found, " + target.position);



                if (target.CompareTag("Untagged"))
                {
                    test = "Button";
                    test = test + textAction;
                    text_Action = color;



                    if (string.Equals(test, textAction))
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
    }

    void ScanVisibleArea()
    {
        
        RequestDecision();
        Debug.Log("Finished ScanVisibleArea");

    }


    private void AgentIdle()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isWalkBack", false);
        anim.SetBool("isTurnLeft", false);
        anim.SetBool("isTurnRight", false);
        anim.SetBool("isPushButton", false);
    }

    private void AgentWalk()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isWalkBack", false);
        anim.SetBool("isTurnLeft", false);
        anim.SetBool("isTurnRight", false);
        anim.SetBool("isPushButton", false);
    }

    private void AgentRightTurn()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isWalkBack", false);
        anim.SetBool("isTurnLeft", false);
        anim.SetBool("isTurnRight", true);
        anim.SetBool("isPushButton", false);
    }

    private void AgentLeftTurn()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isWalkBack", false);
        anim.SetBool("isTurnLeft", true);
        anim.SetBool("isTurnRight", false);
        anim.SetBool("isPushButton", false);
    }

    private void AgentPress()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isWalkBack", false);
        anim.SetBool("isTurnLeft", false);
        anim.SetBool("isTurnRight", false);
        anim.SetBool("isPushButton", true);
    }

    private void AgentWalkBack()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isWalkBack", true);
        anim.SetBool("isTurnLeft", false);
        anim.SetBool("isTurnRight", false);
        anim.SetBool("isPushButton", false);
    }

}
