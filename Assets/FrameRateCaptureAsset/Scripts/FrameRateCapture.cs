using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FrameRateCapture : MonoBehaviour
{
    [Header("Text UI (Text Component)")]
    public Text fpsText;
    public Text captureTimeText;
    public Text msText;

    [Header("Turn on this if you want to test in Unity Editor")]
    public bool isUnityEditor;
    float timer, refresh, avgFramerate, ms;

    float nextActionTime = 0.0f;
    float period = 1f; //detik dalam dunia nyata || jadiin 1 jika ingin 1 detik 
    
    int i=0;
    float []myFPS;

    StreamWriter writer;

    [Header("How many second FPS that you want to captured/ Jumlah berapa detik yang ingin ditampung fps nya")]
    public int manyFrame;


    [Header("The Capture Button / Tombol Capture")]
    public GameObject resetButton;

    [Header("The Test Name / Nama Test ini")]
    public string testName = "First Initiated";

    [Header("Input Field")]
    public InputField inputFieldTestName;
    public InputField inputFieldManyFrame;

    public void RestartFPS () //mengulang fps yang ditampung dalam array (capture button)
    {
        i = 0;
    }

    public void GetTextInputField ()
    {
        testName = inputFieldTestName.text;
        manyFrame = int.Parse(inputFieldManyFrame.text);
    }

 
    void Update()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0)
        {
            avgFramerate = (int)(1f / timelapse);
            ms = 1000 / avgFramerate;
        }
  
        if (Time.time > nextActionTime) 
        {
            nextActionTime = nextActionTime + period;
            fpsText.text = string.Format("FPS " + avgFramerate.ToString());
            captureTimeText.text = string.Format("Capture Time " + i.ToString());
            msText.text = string.Format(i.ToString() + " ms");

            if (i == 0)
            {
                resetButton.SetActive(true);
                myFPS = new float[manyFrame];
                //buat text saat i= 0
                //running in android
                if (isUnityEditor == false) writer = new StreamWriter(Application.persistentDataPath + "FrameRateCaptureResult.txt", true); 
                //running in unity editor
                else writer = new StreamWriter("Assets/FrameRateCaptureResult.txt", true); 
                writer.WriteLine(testName);
            }

            if (i < manyFrame) //angka tangkapan
            {
                resetButton.SetActive(false);
                myFPS[i] = avgFramerate;
                writer.WriteLine(i + " second  | " + avgFramerate + " fps  | " + ms + " ms ");
                //Debug.Log("FPS"+ i + "="+ myFPS[i]);
                i++; 
            }

            if (i == manyFrame)
            {
                writer.Close();
                resetButton.SetActive(true);
            } 
            
        }

    }


}