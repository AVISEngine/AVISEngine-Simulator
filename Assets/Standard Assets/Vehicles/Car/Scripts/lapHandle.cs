using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lapHandle : MonoBehaviour
{   
    public LanguageHandler languageHandler;
    public int CheckpointAmt;
    Text LapText;
    void Start(){
        // languageHandler = new LanguageHandler();
        GameObject gameObject = new GameObject("LanguageHandler");
        languageHandler = gameObject.AddComponent<LanguageHandler>();
        languageHandler.m_dictionary();

        LapText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/LapScrollView/Viewport/Content").GetComponentInChildren<Text>();
        LapText.text = ""; 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<kartLap>())
        {
            //print("LapPassed");
            kartLap car = other.GetComponent<kartLap>();
            car.lapNumber++;
            // LapText.text += "Lap " + (car.lapNumber).ToString() + " : " + (car.checkpointIndex).ToString() + "/" + CheckpointAmt + "\n"; 
            LapText.text += string.Format(languageHandler.dict["Lap"],(car.lapNumber).ToString(),(car.checkpointIndex).ToString(),CheckpointAmt);
            LapText.text += "___________________\n";
            
            car.checkpointIndex = 0;
            car.trueCheckpoint = 0;
            // car.scoresText.text = "Checkpoints and Laps: \n => Laps :" + car.lapNumber.ToString() + "\n => Checkpoints : " + car.checkpointIndex.ToString() + "/" + car.totalCheckpoints.ToString();
            car.scoresText.text = string.Format(languageHandler.dict["LapsAndCheckpoints"],car.lapNumber.ToString(),car.checkpointIndex.ToString(),car.totalCheckpoints.ToString());

            
            // if(car.checkpointIndex == CheckpointAmt)
            // {
            // car.checkpointIndex = 0;
            // car.lapNumber++;
                //print(car.lapNumber);
            // }
        }
    }
}
