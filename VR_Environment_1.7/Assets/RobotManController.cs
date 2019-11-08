using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class RobotManController : MonoBehaviour
{
    [SerializeField]
    Transform _destination;

    public GameObject[] GameButtons;

    NavMeshAgent _navMeshAgent;

    Animator anim;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            SetDestination();
        }
        //_navMeshAgent.updatePosition = false;
    }

    // Update is called once per frame
    private void SetDestination()
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
            //_navMeshAgent.velocity(0,0,2);
        }

        GameButtons = GameObject.FindGameObjectsWithTag("green");
        if (GameButtons != null)
        {
            foreach (GameObject GameButton in GameButtons)
            {
                Vector3 targetVector = GameButton.transform.position;
                _navMeshAgent.SetDestination(targetVector);
                anim.SetBool("isWalking", true);
            }
        }
    }
}
