using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kartLap : MonoBehaviour
{
    public int lapNumber;
    public int checkpointIndex;
    public int totalCheckpoints;
    public int trueCheckpoint; 
    public Text scoresText;
    public LanguageHandler languageHandler;

    // Start is called before the first frame update
    void Start()
    {
        // languageHandler = new LanguageHandler();
        GameObject gameObject = new GameObject("LanguageHandler");
        languageHandler = gameObject.AddComponent<LanguageHandler>();
        languageHandler.m_dictionary();

        lapNumber = 0;
        checkpointIndex = 0;
        trueCheckpoint = 0;
        scoresText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Scores").GetComponentInChildren<Text>();
        // scoresText.text = "Checkpoints and Laps: \n => Laps :" + lapNumber.ToString() + "\n => Checkpoints : " + checkpointIndex.ToString() + "/" + totalCheckpoints.ToString();
        
        scoresText.text = string.Format(languageHandler.dict["LapsAndCheckpoints"],lapNumber.ToString(),checkpointIndex.ToString(),totalCheckpoints.ToString());
    }
}
