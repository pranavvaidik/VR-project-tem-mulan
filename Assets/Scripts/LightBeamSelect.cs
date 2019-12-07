using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;


//[RequireComponent(typeof(SteamVR_LaserPointer))]
public class LightBeamSelect : MonoBehaviour
{
    public SteamVR_Action_Boolean buttonSelect;
    public SteamVR_Input_Sources handType;
    public bool Should_Be_Active;

    public float test = 0.0f;

    public GameObject commandField;

    private SteamVR_LaserPointer laserPointer;




    
    //private SteamVR_

    Transform hitTransform;
    GameObject hitObject;






    //public SteamVR_Action_Boolean interactWithUI = SteamVR_Input.__actions_default_in_InteractUI;

    public bool active = true;
    public Color color;
    public float thickness = 0.002f;
    public Color clickColor = Color.green;
    public Color ClearColor;
    public GameObject holder;
    public GameObject pointer;
    public Transform reference;
    public event PointerEventHandler PointerIn;
    public event PointerEventHandler PointerOut;
    public event PointerEventHandler PointerClick;

    Transform previousContact = null;

    Material laserMaterial;

    // Start is called before the first frame update
    void Start()
    {
        //laserPointer = GetComponent<SteamVR_LaserPointer>();
        buttonSelect.AddOnStateDownListener(OnTriggerDown, handType);

        string m_textCommands = commandField.GetComponent<TextMesh>().text;

        holder = new GameObject();
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;
        holder.transform.localRotation = Quaternion.identity;

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.parent = holder.transform;
        pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
        pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
        pointer.transform.localRotation = Quaternion.identity;
        //BoxCollider collider = pointer.GetComponent<BoxCollider>();

        Destroy(pointer.GetComponent<BoxCollider>());

        laserMaterial = new Material(Shader.Find("Unlit/Color"));
        laserMaterial.SetColor("_Color", color);
        pointer.GetComponent<MeshRenderer>().material = laserMaterial;


    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.forward, out hit) & (Should_Be_Active == true))
        {
            //Debug.Log("We hit something");
            hitTransform = hit.transform;
            hitObject = hitTransform.gameObject;

            if (hitObject.layer == 8)
            {
                if (laserMaterial.color != Color.yellow)
                {
                    laserMaterial.SetColor("_Color", Color.green);
                    pointer.GetComponent<MeshRenderer>().material = laserMaterial;
                }
                else
                {

                }
                

            }
            else
            {
                if (laserMaterial.color != Color.red)
                {
                    laserMaterial.SetColor("_Color", Color.red);
                    pointer.GetComponent<MeshRenderer>().material = laserMaterial;
                }
            }

        }
        else
        {
            //turn off laser
            pointer.GetComponent<MeshRenderer>().material.SetColor("_Color", ClearColor);
            pointer.GetComponent<MeshRenderer>().material.shader = Shader.Find("Transparent/Diffuse");
            //hit.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            //Color tempColor = hit.transform.GetComponent<Renderer>().material.color;
            //tempColor.a = 0;
            //hit.transform.GetComponent<Renderer>().material.color = tempColor;
        }
        
    }



    public void ButtonTrigger()
    {

    }

    public void OnTriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
        // get the position and orientation of the pointer

        string m_textCommands = commandField.GetComponent<TextMesh>().text;

        


        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.forward, out hit) & (Should_Be_Active == true))
        {
            Debug.Log("We hit something");
            hitTransform = hit.transform;
            hitObject = hitTransform.gameObject;

            if (hitObject.layer == 8 && !hitObject.CompareTag("Untagged"))
            {
                laserMaterial.SetColor("_Color", Color.yellow);
                pointer.GetComponent<MeshRenderer>().material = laserMaterial;

                commandField.GetComponent<TextMesh>().text = " ";
                commandField.GetComponent<TextMesh>().text = hitObject.tag;
                /*
                if (m_textCommands == "")
                {
                    commandField.GetComponent<TextMesh>().text += hitObject.tag;
                }
                else
                {
                    commandField.GetComponent<TextMesh>().text += " " + hitObject.tag;

                }
                */
            }

        }
        else
        {
            //turn off laser
            pointer.GetComponent<MeshRenderer>().material.SetColor("_Color", ClearColor);
            pointer.GetComponent<MeshRenderer>().material.shader = Shader.Find("Transparent/Diffuse");
        }
    }

}
