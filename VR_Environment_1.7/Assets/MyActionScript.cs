using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(AudioSource))]

public class MyActionScript : MonoBehaviour
{
    public SteamVR_Action_Boolean RetroMicOnOff; //A reference to the action
    public SteamVR_Input_Sources handType; //A reference to the hand
    public GameObject RetroMic; //Reference to the RetroMic
    private bool micConnected = false; //A boolean that flags whether there's a connected microphone
    private int minFreq; //The maximum and minimum available recording frequencies
    private int maxFreq;
    private AudioSource goAudioSource; //A handle to the attached AudioSource

    void Start()
    {
        RetroMicOnOff.AddOnStateDownListener(TriggerDown, handType);
        RetroMicOnOff.AddOnStateUpListener(TriggerUp, handType);

        if (Microphone.devices.Length <= 0) //Check if there is at least one microphone connected
        {
            Debug.LogWarning("Microphone not connected!"); //Throw a warning message at the console if there isn't
        }
        else //At least one microphone is present
        {
            micConnected = true; //Set 'micConnected' to true
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq); //Get the default microphone recording capabilities

            if (minFreq == 0 && maxFreq == 0) //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...
            {
                maxFreq = 44100; //...meaning 44100 Hz can be used as the recording sampling rate
            }

            goAudioSource = this.GetComponent<AudioSource>(); //Get the attached AudioSource component
        }
    }
    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is up");
        RetroMic.GetComponent<MeshRenderer>().enabled = false;

        Debug.Log("Still connected to mic?");
        if (micConnected) //If there is a microphone
        {
            Debug.Log("Connected to mic, saving");
            SavWav.Save("C:\\Users\\andyj\\VR_Environment_1.4\\Assets\\audio_file", goAudioSource.clip); //Microphone.End(null); //Stop the audio recording
            Microphone.End(null); //Stop the audio recording
            goAudioSource.Play(); //Playback the recorded audio

            //run speech script
            //ExampleStreaming.ExampleStreaming();
        }
        else //No microphone
        {
            Debug.Log("Not connected to mic...");
        }

    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
        RetroMic.GetComponent<MeshRenderer>().enabled = true;

        Debug.Log("Attempting to connect to mic");
        if (micConnected) //If there is a microphone
        {
            Debug.Log("Mic is connected");
            goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq); //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource
        }
        else // No microphone
        {
            Debug.Log("No mic is connected");
        }
    }

    void Update()
    {
    }
}