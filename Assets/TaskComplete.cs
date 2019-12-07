using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskComplete : MonoBehaviour
{
    [SerializeField]
    private Transform AllButtonsStructure; //game object containing all buttons, lights, sounds, etc

    [SerializeField]
    private Transform RoomHallways; //game object containing all buttons, lights, sounds, etc

    Animator animator1;
    Animator animator2;
    Animator animator3;
    Animator animator4;


    // Start is called before the first frame update
    void Start()
    {
        animator1 = RoomHallways.GetChild(0).GetChild(1).GetChild(4).GetComponent<Animator>(); //room 1 user exit
        animator2 = RoomHallways.GetChild(0).GetChild(2).GetChild(4).GetComponent<Animator>(); //room 2 user enter
        animator3 = RoomHallways.GetChild(1).GetChild(1).GetChild(4).GetComponent<Animator>(); //room 1 robot exit
        animator4 = RoomHallways.GetChild(1).GetChild(2).GetChild(4).GetComponent<Animator>(); //room 2 robot enter
    }

    // Update is called once per frame
    void Update()
    {
        int numCorrect1 = System.Convert.ToInt32(AllButtonsStructure.GetChild(0).GetChild(6).GetComponent<TextMesh>().text);
        int numCorrect2 = System.Convert.ToInt32(AllButtonsStructure.GetChild(1).GetChild(6).GetComponent<TextMesh>().text);

        if (numCorrect1 == 5 && numCorrect2 == 5)
        {
            StartCoroutine(StalledSuccess());
        }
    }

    IEnumerator StalledSuccess()
    {
        yield return new WaitForSeconds(2);
        AllButtonsStructure.GetChild(2).GetChild(0).GetComponent<ParticleSystemRenderer>().enabled = true; //turn on confetti source 1
        AllButtonsStructure.GetChild(2).GetChild(1).GetComponent<ParticleSystemRenderer>().enabled = true; //turn on confetti source 2
        AllButtonsStructure.GetChild(2).GetChild(2).GetComponent<ParticleSystemRenderer>().enabled = true; //turn on confetti source 3
        AllButtonsStructure.GetChild(2).GetChild(3).GetComponent<ParticleSystemRenderer>().enabled = true; //turn on confetti source 4
        AllButtonsStructure.GetChild(2).GetComponent<AudioSource>().enabled = true; //play victory sound
        yield return new WaitForSeconds(2);

        //open doors to room 2
        animator1.SetBool("open", true); //open doors
        animator2.SetBool("open", true);
        animator3.SetBool("open", true);
        animator4.SetBool("open", true);

        AllButtonsStructure.GetChild(1).GetChild(0).GetChild(0).tag = "Untagged";
        AllButtonsStructure.GetChild(1).GetChild(1).GetChild(0).tag = "Untagged";
        AllButtonsStructure.GetChild(1).GetChild(2).GetChild(0).tag = "Untagged";
        AllButtonsStructure.GetChild(1).GetChild(3).GetChild(0).tag = "Untagged";
        AllButtonsStructure.GetChild(1).GetChild(4).GetChild(0).tag = "Untagged";
        AllButtonsStructure.GetChild(1).GetChild(5).GetChild(0).tag = "Untagged";
    }
}
