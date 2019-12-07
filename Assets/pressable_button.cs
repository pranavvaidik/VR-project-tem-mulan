using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressable_button : MonoBehaviour
{

    public float endstop; //how far down you want the button to be pressed before it triggers

    [SerializeField]
    private Transform ButtonStructure; //game object contraining buttons, lights, sounds, etc

    private Transform Location;
    private Vector3 StartPos;
    public bool pushed;

    //private GameObject UsrBtn1 = ButtonStructure.GetChild(0);

    void Start()
    {
        Location = GetComponent<Transform>();
        StartPos = Location.position;
    }
    void Update()
    {
        if (GetComponent<Transform>().parent.parent.name == "User_Buttons")
        {
            if (StartPos.x - Location.position.x > endstop) //if button is pushed all the way down
            {
                //print("too low, reset");
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; //lock X movement constraint
                Location.position = new Vector3(StartPos.x - endstop, Location.position.y, Location.position.z); //put it at max pushed position
                GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionX; //Remove X movement constraint

                pushed = true;
                //ButtonFullyPushed(); //check which button was pushed
            }

            if (Location.position.x > StartPos.x) //stop button from being pulled up
            {
                //print("too high, reset");
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; //lock X movement constraint
                Location.position = new Vector3(StartPos.x, Location.position.y, Location.position.z); //reset it
                GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionX; //Remove X movement constraint
            }
        }
        else if (GetComponent<Transform>().parent.parent.name == "Robot_Buttons")
        {
            if (StartPos.x - Location.position.x > endstop) //if button is pushed all the way down
            {
                //print("too low, reset");
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; //lock X movement constraint
                Location.position = new Vector3(StartPos.x - endstop, Location.position.y, Location.position.z); //put it at max pushed position
                GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionX; //Remove X movement constraint

                pushed = true;
                //ButtonFullyPushed(); //check which button was pushed
            }

            if (Location.position.x > StartPos.x) //stop button from being pulled up
            {
                //print("too high, reset");
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; //lock X movement constraint
                Location.position = new Vector3(StartPos.x, Location.position.y, Location.position.z); //reset it
                GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionX; //Remove X movement constraint
            }
        }
    }

    void OnCollisionExit()
    {
        //if (pushed == true)
        bool is_not_pressed = true;
        if (((name == "Inner_Part_1") & ButtonStructure.GetChild(0).GetChild(2).GetChild(0).GetComponent<Light>().enabled) |
            ((name == "Inner_Part_2") & ButtonStructure.GetChild(1).GetChild(2).GetChild(0).GetComponent<Light>().enabled) |
            ((name == "Inner_Part_3") & ButtonStructure.GetChild(2).GetChild(2).GetChild(0).GetComponent<Light>().enabled) |
            ((name == "Inner_Part_4") & ButtonStructure.GetChild(3).GetChild(2).GetChild(0).GetComponent<Light>().enabled) |
            ((name == "Inner_Part_5") & ButtonStructure.GetChild(4).GetChild(2).GetChild(0).GetComponent<Light>().enabled) |
            ((name == "Inner_Part_6") & ButtonStructure.GetChild(5).GetChild(2).GetChild(0).GetComponent<Light>().enabled))
        {
            is_not_pressed = false; //button was already pressed
            Debug.Log("Button is already pressed.");
        }

        if ((pushed == true) & is_not_pressed) //can't press more than once
        {

            Material redOn1 = Resources.Load("BASML_Props 1_1", typeof(Material)) as Material; //user red on
            Material redOn2 = Resources.Load("BASML_Props 1_2", typeof(Material)) as Material; //robot red on
            Material redOff1 = Resources.Load("BASML_Props 2_1", typeof(Material)) as Material; //user red off 1
            Material redOff2 = Resources.Load("BASML_Props 2_2", typeof(Material)) as Material;
            Material redOff3 = Resources.Load("BASML_Props 2_3", typeof(Material)) as Material;
            Material redOff4 = Resources.Load("BASML_Props 2_4", typeof(Material)) as Material;
            Material redOff5 = Resources.Load("BASML_Props 2_5", typeof(Material)) as Material;
            Material redOff6 = Resources.Load("BASML_Props 2_6", typeof(Material)) as Material;

            Material greenOn1 = Resources.Load("BASML_Props 3_1", typeof(Material)) as Material; //user green on
            Material greenOn2 = Resources.Load("BASML_Props 3_1", typeof(Material)) as Material; //robot green on

            //print(name);
            ButtonStructure.GetChild(0).GetComponent<AudioSource>().enabled = false; //turn off sound on all buttons
            ButtonStructure.GetChild(1).GetComponent<AudioSource>().enabled = false;
            ButtonStructure.GetChild(2).GetComponent<AudioSource>().enabled = false;
            ButtonStructure.GetChild(3).GetComponent<AudioSource>().enabled = false;
            ButtonStructure.GetChild(4).GetComponent<AudioSource>().enabled = false;
            ButtonStructure.GetChild(5).GetComponent<AudioSource>().enabled = false;

            ButtonStructure.GetChild(0).GetChild(2).GetChild(0).GetComponent<Light>().enabled = false; //turn off all button lights
            ButtonStructure.GetChild(1).GetChild(2).GetChild(0).GetComponent<Light>().enabled = false;
            ButtonStructure.GetChild(2).GetChild(2).GetChild(0).GetComponent<Light>().enabled = false;
            ButtonStructure.GetChild(3).GetChild(2).GetChild(0).GetComponent<Light>().enabled = false;
            ButtonStructure.GetChild(4).GetChild(2).GetChild(0).GetComponent<Light>().enabled = false;
            ButtonStructure.GetChild(5).GetChild(2).GetChild(0).GetComponent<Light>().enabled = false;

            ButtonStructure.GetChild(0).GetChild(2).GetComponent<Renderer>().material = redOff1; //change material of all button lights to red off
            ButtonStructure.GetChild(1).GetChild(2).GetComponent<Renderer>().material = redOff2;
            ButtonStructure.GetChild(2).GetChild(2).GetComponent<Renderer>().material = redOff3;
            ButtonStructure.GetChild(3).GetChild(2).GetComponent<Renderer>().material = redOff4;
            ButtonStructure.GetChild(4).GetChild(2).GetComponent<Renderer>().material = redOff5;
            ButtonStructure.GetChild(5).GetChild(2).GetComponent<Renderer>().material = redOff6;

            //toggle lights and sounds
            if (name == "Inner_Part_1")
            {
                ButtonStructure.GetChild(0).GetChild(2).GetChild(0).GetComponent<Light>().enabled = true;  //turn on light on button 1
                ButtonStructure.GetChild(0).GetChild(2).GetComponent<Renderer>().material = redOn1; //change material of button 1 light to red on
                ButtonStructure.GetChild(0).GetComponent<AudioSource>().enabled = true; //play note 1
            }
            else if (name == "Inner_Part_2")
            {
                ButtonStructure.GetChild(1).GetChild(2).GetChild(0).GetComponent<Light>().enabled = true;  //turn on light on button 2
                ButtonStructure.GetChild(1).GetChild(2).GetComponent<Renderer>().material = redOn1; //change material of button 2 light to red on
                ButtonStructure.GetChild(1).GetComponent<AudioSource>().enabled = true; //play note 2
            }
            else if (name == "Inner_Part_3")
            {
                ButtonStructure.GetChild(2).GetChild(2).GetChild(0).GetComponent<Light>().enabled = true;  //turn on light on button 3
                ButtonStructure.GetChild(2).GetChild(2).GetComponent<Renderer>().material = redOn1; //change material of button 3 light to red on
                ButtonStructure.GetChild(2).GetComponent<AudioSource>().enabled = true; //play note 3
            }
            else if (name == "Inner_Part_4")
            {
                ButtonStructure.GetChild(3).GetChild(2).GetChild(0).GetComponent<Light>().enabled = true;  //turn on light on button 4
                ButtonStructure.GetChild(3).GetChild(2).GetComponent<Renderer>().material = redOn1; //change material of button 4 light to red on
                ButtonStructure.GetChild(3).GetComponent<AudioSource>().enabled = true; //play note 4
            }
            else if (name == "Inner_Part_5")
            {
                ButtonStructure.GetChild(4).GetChild(2).GetChild(0).GetComponent<Light>().enabled = true;  //turn on light on button 5
                ButtonStructure.GetChild(4).GetChild(2).GetComponent<Renderer>().material = redOn1; //change material of button 5 light to red on
                ButtonStructure.GetChild(4).GetComponent<AudioSource>().enabled = true; //play note 5
            }
            else if (name == "Inner_Part_6")
            {
                ButtonStructure.GetChild(5).GetChild(2).GetChild(0).GetComponent<Light>().enabled = true;  //turn on light on button 6
                ButtonStructure.GetChild(5).GetChild(2).GetComponent<Renderer>().material = redOn1; //change material of button 6 light to red on
                ButtonStructure.GetChild(5).GetComponent<AudioSource>().enabled = true; //play note 6
            }
            else
            {
                Debug.Log("Unknown button pressed.");
            }



            string rawText = ButtonStructure.GetChild(7).GetComponent<TextMesh>().text; //the 5 button sequence
            string[] numbers = rawText.Split(',');
            int[] correct_seq = Array.ConvertAll(numbers, int.Parse);

            int numCorrect = System.Convert.ToInt32(ButtonStructure.GetChild(6).GetComponent<TextMesh>().text); //number currently correct
            Debug.Log("Looking for button: " + correct_seq[numCorrect]);
            //Debug.Log(correct_seq[numCorrect]);


            if ((name == "Inner_Part_1" && correct_seq[numCorrect] == 1) ||
                (name == "Inner_Part_2" && correct_seq[numCorrect] == 2) ||
                (name == "Inner_Part_3" && correct_seq[numCorrect] == 3) ||
                (name == "Inner_Part_4" && correct_seq[numCorrect] == 4) ||
                (name == "Inner_Part_5" && correct_seq[numCorrect] == 5) ||
                (name == "Inner_Part_6" && correct_seq[numCorrect] == 6)) //if the next correct button was pushed
            {
                Debug.Log("Just pressed button " + correct_seq[numCorrect]);
                ButtonStructure.GetChild(6).GetChild(numCorrect).GetChild(0).GetComponent<Light>().enabled = true;//turn next light on
                ButtonStructure.GetChild(6).GetChild(numCorrect).GetComponent<Renderer>().material = greenOn1; //change material of sequence light to green on
                ButtonStructure.GetChild(6).GetComponent<TextMesh>().text = (numCorrect + 1).ToString(); ; //increment number correct
                numCorrect += 1;
            }
            else
            {
                //wrong button was pushed, but it is the 1st button in the sequence
                if ((name == "Inner_Part_1" && correct_seq[0] == 1) ||
                    (name == "Inner_Part_2" && correct_seq[0] == 2) ||
                    (name == "Inner_Part_3" && correct_seq[0] == 3) ||
                    (name == "Inner_Part_4" && correct_seq[0] == 4) ||
                    (name == "Inner_Part_5" && correct_seq[0] == 5) ||
                    (name == "Inner_Part_6" && correct_seq[0] == 6)) //if the next correct button was pushed
                {
                    Debug.Log("Just pressed button " + correct_seq[0]);
                    ButtonStructure.GetChild(6).GetChild(0).GetChild(0).GetComponent<Light>().enabled = true;//turn next light on
                    ButtonStructure.GetChild(6).GetChild(0).GetComponent<Renderer>().material = greenOn1; //change material of sequence light to green on

                    //Material greenOff1 = Resources.Load("BASML_Props 4_1", typeof(Material)) as Material; //user green off 1
                    Material greenOff2 = Resources.Load("BASML_Props 4_2", typeof(Material)) as Material;
                    Material greenOff3 = Resources.Load("BASML_Props 4_3", typeof(Material)) as Material;
                    Material greenOff4 = Resources.Load("BASML_Props 4_4", typeof(Material)) as Material;
                    Material greenOff5 = Resources.Load("BASML_Props 4_5", typeof(Material)) as Material;
                    //ButtonStructure.GetChild(6).GetChild(0).GetChild(0).GetComponent<Light>().enabled = false;//turn all lights off
                    ButtonStructure.GetChild(6).GetChild(1).GetChild(0).GetComponent<Light>().enabled = false;
                    ButtonStructure.GetChild(6).GetChild(2).GetChild(0).GetComponent<Light>().enabled = false;
                    ButtonStructure.GetChild(6).GetChild(3).GetChild(0).GetComponent<Light>().enabled = false;
                    ButtonStructure.GetChild(6).GetChild(4).GetChild(0).GetComponent<Light>().enabled = false;
                    //ButtonStructure.GetChild(6).GetChild(0).GetComponent<Renderer>().material = greenOff1; //change material of sequence light 1 to green off
                    ButtonStructure.GetChild(6).GetChild(1).GetComponent<Renderer>().material = greenOff2; //change material of sequence light 2 to green off
                    ButtonStructure.GetChild(6).GetChild(2).GetComponent<Renderer>().material = greenOff3; //change material of sequence light 3 to green off
                    ButtonStructure.GetChild(6).GetChild(3).GetComponent<Renderer>().material = greenOff4; //change material of sequence light 4 to green off
                    ButtonStructure.GetChild(6).GetChild(4).GetComponent<Renderer>().material = greenOff5; //change material of sequence light 5 to green off

                    ButtonStructure.GetChild(6).GetComponent<TextMesh>().text = (1).ToString(); ; //increment number correct
                    numCorrect = 1;
                }
                else
                {



                    Material greenOff1 = Resources.Load("BASML_Props 4_1", typeof(Material)) as Material; //user green off 1
                    Material greenOff2 = Resources.Load("BASML_Props 4_2", typeof(Material)) as Material;
                    Material greenOff3 = Resources.Load("BASML_Props 4_3", typeof(Material)) as Material;
                    Material greenOff4 = Resources.Load("BASML_Props 4_4", typeof(Material)) as Material;
                    Material greenOff5 = Resources.Load("BASML_Props 4_5", typeof(Material)) as Material;
                    ButtonStructure.GetChild(6).GetChild(0).GetChild(0).GetComponent<Light>().enabled = false;//turn all lights off
                    ButtonStructure.GetChild(6).GetChild(1).GetChild(0).GetComponent<Light>().enabled = false;
                    ButtonStructure.GetChild(6).GetChild(2).GetChild(0).GetComponent<Light>().enabled = false;
                    ButtonStructure.GetChild(6).GetChild(3).GetChild(0).GetComponent<Light>().enabled = false;
                    ButtonStructure.GetChild(6).GetChild(4).GetChild(0).GetComponent<Light>().enabled = false;
                    ButtonStructure.GetChild(6).GetChild(0).GetComponent<Renderer>().material = greenOff1; //change material of sequence light 1 to green off
                    ButtonStructure.GetChild(6).GetChild(1).GetComponent<Renderer>().material = greenOff2; //change material of sequence light 2 to green off
                    ButtonStructure.GetChild(6).GetChild(2).GetComponent<Renderer>().material = greenOff3; //change material of sequence light 3 to green off
                    ButtonStructure.GetChild(6).GetChild(3).GetComponent<Renderer>().material = greenOff4; //change material of sequence light 4 to green off
                    ButtonStructure.GetChild(6).GetChild(4).GetComponent<Renderer>().material = greenOff5; //change material of sequence light 5 to green off
                    ButtonStructure.GetChild(6).GetComponent<TextMesh>().text = "0"; //reset correct counter
                    numCorrect = 0;
                    Debug.Log("Wrong Button Pushed");
                }
            }
            pushed = false; //reset button
        }
    }
}