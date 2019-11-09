using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class MovementController : MonoBehaviour
{
    private Animator Anim;
    public float speed = 0.001F;
    public float rotationSpeed = 100.0F;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        TextAsset SourceFile = (TextAsset)Resources.Load("Test.txt", typeof(TextAsset));
        string data = SourceFile.text;
        Debug.Log("data" + data);
        string[] commands = Regex.Split(data, "\r|\n|\r\n");

        string[] knownOperations = new string[] { "Find", "Scan", "Fetch", "Grab", "Release", "Pick up", "Put down", "Press", "Go to", "describe" };  //Known operations as of now: Find, Scan, Fetch, Grab, Release
        string[] knownColors = new string[] { "red", "green", "blue", "yellow" };
        string[] knownShapes = new string[] { "button" };
        string[] knownLocation = new string[] { "" };

        for (int i = 0; i < commands.Length; i++)
        {
            string line = commands[i];
            string[] attributes = line.Split(',');
            int operationPos = -1;
            int colorPos = -1;
            int shapePos = -1;
            int locPos = -1;

            for (int j = 0; j < attributes.Length; j++)
            {
                Debug.Log("Attributes" + attributes[j]);

                if (operationPos == -1)
                    operationPos = Array.IndexOf(knownOperations, attributes[j]);
                if (colorPos == -1)
                    colorPos = Array.IndexOf(knownColors, attributes[j]);
                if (shapePos == -1)
                    shapePos = Array.IndexOf(knownShapes, attributes[j]);

                Debug.Log("operationPos" + operationPos);
                Debug.Log("colorPos" + colorPos);
                Debug.Log("shapePos" + shapePos);
            }

            string operation = operationPos > -1 ? knownOperations[operationPos] : "";
            string color = colorPos > -1 ? knownColors[colorPos] : "";
            string shape = shapePos > -1 ? knownShapes[shapePos] : "";

            Debug.Log("operation" + operation);
            Debug.Log("color" + color);
            Debug.Log("shape" + shape);

            if (shape.Equals("button") && color.Equals("yellow"))
            {
                moveForward();
                Debug.Log("Moving forward");
            }
            if (shape.Equals("sphere"))
            {
                moveBackWard();
            }
        }

        if (translation > 0)
        {
            moveForward();
        }
        else if (translation < 0)
        {
            moveBackWard();
        }
        else if (rotation > 0)
        {
            turnLeft();
        }
        else if (rotation < 0)
        {
            turnRight();
        }
        else
        {
            standIdle();
        }
    }

    void moveForward()
    {
        Anim.SetBool("isWalking", true);
        Anim.SetBool("isWalkBack", false);
        Anim.SetBool("isTurnLeft", false);
        Anim.SetBool("isTurnRight", false);
    }

    void moveBackWard()
    {
        Anim.SetBool("isWalking", false);
        Anim.SetBool("isWalkBack", true);
        Anim.SetBool("isTurnLeft", false);
        Anim.SetBool("isTurnRight", false);
    }

    void turnLeft()
    {
        Anim.SetBool("isWalking", false);
        Anim.SetBool("isWalkBack", false);
        Anim.SetBool("isTurnLeft", true);
        Anim.SetBool("isTurnRight", false);
    }

    void turnRight()
    {
        Anim.SetBool("isWalking", false);
        Anim.SetBool("isWalkBack", false);
        Anim.SetBool("isTurnLeft", false);
        Anim.SetBool("isTurnRight", true);
    }

    void standIdle()
    {
        Anim.SetBool("isWalking", false);
        Anim.SetBool("isWalkBack", false);
        Anim.SetBool("isTurnLeft", false);
        Anim.SetBool("isTurnRight", false);
    }
}
