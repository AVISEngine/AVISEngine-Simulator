using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using ArabicSupport;
public class CalibrationUI : MonoBehaviour
{
    //UI Elements
    Toggle obstacleToggle;
    Canvas cgMain;
    Canvas cgPanel;
    Button reset;
    Button close;
    Button quit;
    Text Help;
    public GameObject MainCanvas;

    //Classes
	public LanguageHandler languageHandler;

    //Variables
    private bool ismainPanel = true;

    void Start()
    {
        //Buttons
        reset = GameObject.Find("UIPanelCal/Reset").GetComponentInChildren<Button>();
        quit = GameObject.Find("UIPanelCal/Levels").GetComponentInChildren<Button>();
        Help = GameObject.Find("UIPanelCal/Scroll View/Viewport/Content/HelpText").GetComponent<Text>();
        
        //Button Events
        reset.onClick.AddListener(resetEvent);
        quit.onClick.AddListener(quitEvent);
        
        
        //Language Handler
        GameObject gameObject = new GameObject("LanguageHandler");
        languageHandler = gameObject.AddComponent<LanguageHandler>();
        // languageHandler = new LanguageHandler();
        languageHandler.m_dictionary();
    
        if(languageHandler.lang == "fa"){
            Help.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["Help"], false, false);
            reset.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["Reset"], false, false);
            quit.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["QuitToMenu"], false, false);
        }else{
            Help.GetComponentInChildren<Text>().text = languageHandler.dict["Help"];
            reset.GetComponentInChildren<Text>().text = languageHandler.dict["Reset"];
            quit.GetComponentInChildren<Text>().text = languageHandler.dict["QuitToMenu"];
        }
        

    }

    void quitEvent()
    {
        SceneManager.LoadScene("NewMainMenu");
    }
    void resetEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

