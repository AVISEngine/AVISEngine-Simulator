using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lapCheckPoint : MonoBehaviour
{
    public int Index;
    public LanguageHandler languageHandler;
    void Start(){
        // languageHandler = new LanguageHandler();
        GameObject gameObject = new GameObject("LanguageHandler");
        languageHandler = gameObject.AddComponent<LanguageHandler>();
        languageHandler.m_dictionary();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<kartLap>())
        {
            kartLap car = other.GetComponent<kartLap>();
            // if (car.checkpointIndex == Index + 1 || car.checkpointIndex == Index - 1)
            if ((car.checkpointIndex == Index + 1 || car.checkpointIndex == Index - 1) && car.trueCheckpoint != Index && Index > car.trueCheckpoint)
            {
                car.checkpointIndex = Index;
                car.trueCheckpoint = Index;
            }else{
                if(car.trueCheckpoint != Index && Index > car.trueCheckpoint){
                    car.checkpointIndex = car.checkpointIndex + 1;
                    car.trueCheckpoint = Index;
                }
            }
            // print("________________");
            // print(Index);
            // print(car.trueCheckpoint);
            // print(car.checkpointIndex);
            // print("________________");
            // car.scoresText.text = "Checkpoints and Laps: \n => Laps :" + car.lapNumber.ToString() + "\n => Checkpoints : " + car.checkpointIndex.ToString() + "/" + car.totalCheckpoints.ToString();
            car.scoresText.text = string.Format(languageHandler.dict["LapsAndCheckpoints"],car.lapNumber.ToString(),car.checkpointIndex.ToString(),car.totalCheckpoints.ToString());

        }
    }
}
