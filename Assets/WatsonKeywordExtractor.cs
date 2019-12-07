#pragma warning disable 0649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using IBM.Watson.SpeechToText.V1;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.DataTypes;

[RequireComponent(typeof(AudioSource))]

public class WatsonKeywordExtractor : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Space(10)]
    [Tooltip("The service URL (optional). This defaults to \"https://stream.watsonplatform.net/speech-to-text/api\"")]
    [SerializeField]
    private string _serviceUrl;
    [Tooltip("Text field to display the results of streaming.")]
    public TextMesh ResultsField;
    [Tooltip("Text object where the parsed command will be held.")]
    public TextMesh CommandField;
    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string _iamApikey;

    [Header("Parameters")]
    // https://www.ibm.com/watson/developercloud/speech-to-text/api/v1/curl.html?curl#get-model
    [Tooltip("The Model to use. This defaults to en-US_BroadbandModel")]
    [SerializeField]
    private string _recognizeModel;
    #endregion


    private int _recordingRoutine = 0;
    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingBufferSize = 1;
    private int _recordingHZ = 22050;

    private SpeechToTextService _service;

    public SteamVR_Action_Boolean RetroMicOnOff; //A reference to the action
    public SteamVR_Input_Sources handType; //A reference to the hand
    public GameObject RetroMic; //Reference to the the retro mic model
    public GameObject HologramEmitter; //Reference to the the holographic emitter, audio, and particles
    public bool Should_Be_Active;

    private bool micConnected = false; //A boolean that flags whether there's a connected microphone
    private int minFreq; //The maximum and minimum available recording frequencies
    private int maxFreq;
    private AudioSource goAudioSource; //A handle to the attached AudioSource

    void Start()
    {
        RetroMicOnOff.AddOnStateDownListener(TriggerDown, handType);
        RetroMicOnOff.AddOnStateUpListener(TriggerUp, handType);

        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());

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

    private IEnumerator CreateService()
    {
        if (string.IsNullOrEmpty(_iamApikey))
        {
            throw new IBMException("Plesae provide IAM ApiKey for the service.");
        }

        IamAuthenticator authenticator = new IamAuthenticator(apikey: _iamApikey);

        //  Wait for tokendata
        while (!authenticator.CanAuthenticate())
            yield return null;

        _service = new SpeechToTextService(authenticator);
        if (!string.IsNullOrEmpty(_serviceUrl))
        {
            _service.SetServiceUrl(_serviceUrl);
        }
        _service.StreamMultipart = true;

        Active = true;
        //StartRecording();
    }

    public bool Active
    {
        get { return _service.IsListening; }
        set
        {
            if (value && !_service.IsListening)
            {
                _service.RecognizeModel = (string.IsNullOrEmpty(_recognizeModel) ? "en-US_BroadbandModel" : _recognizeModel);
                _service.DetectSilence = true;
                _service.EnableWordConfidence = true;
                _service.EnableTimestamps = true;
                _service.SilenceThreshold = 0.01f;
                _service.MaxAlternatives = 1;
                _service.EnableInterimResults = true;
                _service.OnError = OnError;
                _service.InactivityTimeout = -1;
                _service.ProfanityFilter = false;
                _service.SmartFormatting = true;
                _service.SpeakerLabels = false;
                _service.WordAlternativesThreshold = null;
                _service.StartListening(OnRecognize, OnRecognizeSpeaker);
            }
            else if (!value && _service.IsListening)
            {
                _service.StopListening();
            }
        }
    }

    private void StartRecording()
    {
        if (_recordingRoutine == 0)
        {
            UnityObjectUtil.StartDestroyQueue();
            _recordingRoutine = Runnable.Run(RecordingHandler());
        }
    }

    private void StopRecording()
    {
        if (_recordingRoutine != 0)
        {
            Microphone.End(_microphoneID);
            Runnable.Stop(_recordingRoutine);
            _recordingRoutine = 0;
        }
    }

    private void OnError(string error)
    {
        Active = false;

        Log.Debug("ExampleStreaming.OnError()", "Error! {0}", error);
    }

    private IEnumerator RecordingHandler()
    {
        Log.Debug("ExampleStreaming.RecordingHandler()", "devices: {0}", Microphone.devices);
        _recording = Microphone.Start(_microphoneID, true, _recordingBufferSize, _recordingHZ);
        yield return null;      // let _recordingRoutine get set..

        if (_recording == null)
        {
            StopRecording();
            yield break;
        }

        bool bFirstBlock = true;
        int midPoint = _recording.samples / 2;
        float[] samples = null;

        while (_recordingRoutine != 0 && _recording != null)
        {
            int writePos = Microphone.GetPosition(_microphoneID);
            if (writePos > _recording.samples || !Microphone.IsRecording(_microphoneID))
            {
                Log.Error("ExampleStreaming.RecordingHandler()", "Microphone disconnected.");

                StopRecording();
                yield break;
            }

            if ((bFirstBlock && writePos >= midPoint)
              || (!bFirstBlock && writePos < midPoint))
            {
                // front block is recorded, make a RecordClip and pass it onto our callback.
                samples = new float[midPoint];
                _recording.GetData(samples, bFirstBlock ? 0 : midPoint);

                AudioData record = new AudioData();
                record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
                record.Clip = AudioClip.Create("Recording", midPoint, _recording.channels, _recordingHZ, false);
                record.Clip.SetData(samples, 0);

                _service.OnListen(record);

                bFirstBlock = !bFirstBlock;
            }
            else
            {
                // calculate the number of samples remaining until we ready for a block of audio, 
                // and wait that amount of time it will take to record.
                int remaining = bFirstBlock ? (midPoint - writePos) : (_recording.samples - writePos);
                float timeRemaining = (float)remaining / (float)_recordingHZ;

                yield return new WaitForSeconds(timeRemaining);
            }
        }
        yield break;
    }

    private void OnRecognize(SpeechRecognitionEvent result)
    {
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    //string text = string.Format("{0} ({1}, {2:0.00})\n", alt.transcript, res.final ? "Final" : "Interim", alt.confidence);
                    string text = string.Format("{0}\n", alt.transcript);
                    Log.Debug("ExampleStreaming.OnRecognize()", text);

                    //search text
                    string colored_text = "";
                    string command_text = "";
                    foreach (string word in text.Split(' '))
                    {
                        if (word == "press")
                        {
                            colored_text += "<color=green><b>press</b></color> ";
                            //command_text += "press ";
                        }
                        else if (word == "blue")
                        {
                            colored_text += "<color=green><b>blue</b></color> ";
                            command_text += "blue ";
                        }
                        else if (word == "red")
                        {
                            colored_text += "<color=green><b>red</b></color> ";
                            command_text += "red ";
                        }
                        else if (word == "orange")
                        {
                            colored_text += "<color=green><b>orange</b></color> ";
                            command_text += "red ";
                        }
                        else if (word == "green")
                        {
                            colored_text += "<color=green><b>green</b></color> ";
                            command_text += "green ";
                        }
                        else if (word == "yellow")
                        {
                            colored_text += "<color=green><b>yellow</b></color> ";
                            command_text += "yellow ";
                        }
                        else if (word == "purple")
                        {
                            colored_text += "<color=green><b>purple</b></color> ";
                            command_text += "purple ";
                        }
                        else if (word == "pink")
                        {
                            colored_text += "<color=green><b>pink</b></color> ";
                            command_text += "purple ";
                        }
                        else if (word == "cyan")
                        {
                            colored_text += "<color=green><b>cyan</b></color> ";
                            command_text += "cyan ";
                        }
                        else if (word == "teal")
                        {
                            colored_text += "<color=green><b>teal</b></color> ";
                            command_text += "cyan ";
                        }
                        else if (word == "turquoise")
                        {
                            colored_text += "<color=green><b>turquoise</b></color> ";
                            command_text += "cyan ";
                        }
                        else if (word == "sky")
                        {
                            colored_text += "<color=green><b>sky</b></color> ";
                            command_text += "cyan ";
                        }
                        else if (word == "button")
                        {
                            colored_text += "<color=green><b>button</b></color> ";
                            //command_text += "button ";
                        }
                        else
                        {
                            colored_text += word + " ";
                        }
                    }

                    // check if command is valid, else show problems
                    ResultsField.text = colored_text; //show text
                    Log.Debug("The command is: ", command_text); //load command array <----
                    CommandField.GetComponent<TextMesh>().text = ""; //clear the command text
                    CommandField.GetComponent<TextMesh>().text = command_text; //set the command text
                }

                if (res.keywords_result != null && res.keywords_result.keyword != null)
                {
                    foreach (var keyword in res.keywords_result.keyword)
                    {
                        Log.Debug("ExampleStreaming.OnRecognize()", "keyword: {0}, confidence: {1}, start time: {2}, end time: {3}", keyword.normalized_text, keyword.confidence, keyword.start_time, keyword.end_time);
                    }
                }

                if (res.word_alternatives != null)
                {
                    foreach (var wordAlternative in res.word_alternatives)
                    {
                        Log.Debug("ExampleStreaming.OnRecognize()", "Word alternatives found. Start time: {0} | EndTime: {1}", wordAlternative.start_time, wordAlternative.end_time);
                        foreach (var alternative in wordAlternative.alternatives)
                            Log.Debug("ExampleStreaming.OnRecognize()", "\t word: {0} | confidence: {1}", alternative.word, alternative.confidence);
                    }
                }
            }
        }
    }

    private void OnRecognizeSpeaker(SpeakerRecognitionEvent result)
    {
        if (result != null)
        {
            foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
            {
                Log.Debug("ExampleStreaming.OnRecognizeSpeaker()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
            }
        }
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is up");

        if (Should_Be_Active == true)
        {
            ResultsField.text = ""; //clear text field
            RetroMic.GetComponent<MeshRenderer>().enabled = false; //make holographic mic invisible
            HologramEmitter.GetComponent<AudioSource>().volume = 0; //turn off sound
            HologramEmitter.GetComponent<ParticleSystemRenderer>().enabled = false; //turn off sound

            Debug.Log("Still connected to mic?");
            if (micConnected) //If there is a microphone
            {
                Debug.Log("Stopping translation");
                StopRecording();
            }
            else //No microphone
            {
                Debug.Log("Not connected to mic...");
            }
            _service.StopListening();
        }
    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (Should_Be_Active == true)
        {
            _service.StartListening(OnRecognize, OnRecognizeSpeaker);
            Debug.Log("Trigger is down");
            RetroMic.GetComponent<MeshRenderer>().enabled = true; //make holographic mic visible
            HologramEmitter.GetComponent<AudioSource>().volume = 1; //turn sound on
            HologramEmitter.GetComponent<ParticleSystemRenderer>().enabled = true; //turn off sound
            Debug.Log("Attempting to connect to mic");
            if (micConnected) //If there is a microphone
            {
                Debug.Log("Mic is connected");
                StartRecording();
            }
            else // No microphone
            {
                Debug.Log("No mic is connected");
            }
        }
    }

    void Update()
    {
    }
}