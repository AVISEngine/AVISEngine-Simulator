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
    public float lastCheckpointTime;
    public string teamName;
    public string stageName;
    public Text scoresText;
    public LanguageHandler languageHandler;

    private InputField teamNameInput;
    private InputField stageNameInput;

    // Start is called before the first frame update
    void Start()
    {
        // languageHandler = new LanguageHandler();
        GameObject gameObject = new GameObject("LanguageHandler");
        languageHandler = gameObject.AddComponent<LanguageHandler>();
        languageHandler.m_dictionary();
        lastCheckpointTime = Time.time;
        
        teamNameInput = GameObject.Find("UIPanel/TeamName").GetComponentInChildren<InputField>();
        stageNameInput = GameObject.Find("UIPanel/StageName").GetComponentInChildren<InputField>();

        teamName = teamNameInput.text;
        stageName = stageNameInput.text;

        // Add listeners to update values when input changes
        teamNameInput.onValueChanged.AddListener(UpdateTeamName);
        stageNameInput.onValueChanged.AddListener(UpdateStageName);

        lapNumber = 0;
        checkpointIndex = 0;
        trueCheckpoint = 0;
        scoresText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Scores").GetComponentInChildren<Text>();
        // scoresText.text = "Checkpoints and Laps: \n => Laps :" + lapNumber.ToString() + "\n => Checkpoints : " + checkpointIndex.ToString() + "/" + totalCheckpoints.ToString();
        
        scoresText.text = string.Format(languageHandler.dict["LapsAndCheckpoints"],lapNumber.ToString(),checkpointIndex.ToString(),totalCheckpoints.ToString());
    }

    private void UpdateTeamName(string newTeamName)
    {
        teamName = newTeamName;
    }

    private void UpdateStageName(string newStageName)
    {
        stageName = newStageName;
    }
}
