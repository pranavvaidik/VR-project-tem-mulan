using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class NavigationDirector : MonoBehaviour
{
    GameObject[] GameButtons;

    NavMeshAgent _navMeshAgent;
    Animator anim;

    List<Vector3> TravelPositions = new List<Vector3>();
    int DestNum = 0;

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    private float waitTime = 2.0f;
    private float timer = 0.0f;

    [Tooltip("Text object where the parsed command will be held.")]
    public TextMesh CommandField;

    // Get list of colors to navigate to - [green, yellow, blue] from Andrew
    //string[] ButtonOrder = new string[] { "green" };

    bool ispressing = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        Debug.Log("Command Field" + CommandField.text);

        if (_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            SetDestinations();
            //GoToNextPoint();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("The nav mesh agent is at position " + _navMeshAgent.transform.position);
        //Debug.Log("The first button is at position" + TravelPositions[0]);
        //GoToNextPoint();

        //AgentIdle();
        if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
        {
            AgentIdle();
        }
        else if (_navMeshAgent.remainingDistance == 0)
        {
            AgentPress();   //
        }               
        else 
        {
            AgentWalk();
        }

        // Choose the next destination point when the agent gets close to the current one.
        //&& _navMeshAgent.remainingDistance < 0.5f
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.2f)
        {
            ispressing = true;
            AgentPress();
            //GoToNextPoint();
        }
    }

    private void SetDestinations()
    {
        string data = CommandField.text;
        char[] separator = { ' ', ',' };
        string[] colors = data.Split(separator);
        vision_agent vision_Agent = new vision_agent();
        foreach (string color in colors)
        {
            GameButtons = GameObject.FindGameObjectsWithTag(color);
            if (GameButtons == null) {
                vision_Agent.ScanVisibleArea();
                GameButtons = GameObject.FindGameObjectsWithTag(color);
            }
            if (GameButtons != null)
            {
                foreach (GameObject GameButton in GameButtons)
                {
                    Vector3 targetVector = GameButton.transform.position;
                    targetVector[0] = targetVector[0] + 0.77f;   //Adding offset based on Arms length
                    targetVector[1] = 0.0f;  //Robot moves only in x and z direction
                    targetVector[2] = targetVector[2] - 0.2f;   //Adding offset based on Arms length
                    TravelPositions.Add(targetVector);
                    GoToNextPoint();
                }
            }
            else
            {
                Debug.Log("Game Object not found with color " + color);
            }
        }
        Debug.Log("Travel Positions: " + TravelPositions.Count);
    }

    private void GoToNextPoint()
    {
        //Check if there are points to walk to 
        if (TravelPositions.Count == 0)
            Debug.Log("No TravelPositions");

        // Set the agent to go to the currently selected destination.
        _navMeshAgent.SetDestination(TravelPositions[DestNum]);

        // Choose the next point in the array as the destination
        DestNum += 1;
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
