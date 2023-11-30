using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ArabicSupport;
//Changelog
/*
Version 1.0.8
1.Added Language handler
2.Multilingual Support
*/
public class UIController : MonoBehaviour
{
	// UI Elements
	public Button Race1;
	public Button Race2;
	public Button Race3;
	public Button Urban1;
	public Button Urban2;
	public Button Urban3;
	public Button Exit;
	public Button Github;
	public Button Terms;
	public Button AboutSim;
	public Button Calib;
	public Button backButton;
	public Text versionText;
	public Text headerText;
	public Text aboutHeaderText;
	public Canvas aboutCanvas;
	Button btn1;
	Button btn2;
	Button btn3;
	Button btn4;
	Button btn5;
	Button btn6;
	Button exitButton;
	Button calibButton;
	Button openGithub;
	Button terms;
	Button aboutSimBtn;
	Button back;
	Canvas canvasAbout;
	// Classes
	public LanguageHandler languageHandler;

	// Version Settings
	// string versionTextStr = "FIRA Autonomous Cars : Tele-Operation Simulator Ver ";
	// Last change : Jan 25 -> Robocup edition, Feb 7 -> Camera Calibration + Traffic System
	bool isFinalVersion = true; 
	bool isRealVersion = false;
	bool isAlphaVersion = false;
	string version = "1.2.7";
	string finalVersion = "1.2.7";
	string versionTextStr = " pre-alpha";
	string versionResult;
	string githubURL = "https://github.com/AvisEngine/AVIS-Engine-Python-API";
	string termsURL = "http://avisengine.com/225286-2/";
    public Dropdown dropDown;
	public Image translatedIcon;
	string termsOfUse = "";
    public Sprite[] flags;

	void Start()
	{	
		// Buttons
		btn1 = Race1.GetComponent<Button>();
		btn2 = Race2.GetComponent<Button>();
		btn3 = Urban2.GetComponent<Button>();
		btn4 = Urban1.GetComponent<Button>();
		btn5 = Urban3.GetComponent<Button>();
		btn6 = Race3.GetComponent<Button>();

		exitButton = Exit.GetComponent<Button>();
		calibButton = Calib.GetComponent<Button>();
		openGithub = Github.GetComponent<Button>();
		aboutSimBtn = AboutSim.GetComponent<Button>();

		terms = Terms.GetComponent<Button>();
		back = backButton.GetComponent<Button>();
		canvasAbout = aboutCanvas.GetComponent<Canvas>();

		Hide(canvasAbout);

		//Buttons Events
		btn1.onClick.AddListener(Race1Event);
		btn2.onClick.AddListener(Race2Event);
		btn3.onClick.AddListener(Urban1Event);
		btn3.Select();
		btn4.onClick.AddListener(Urban2Event);
		btn5.onClick.AddListener(Urban3Event);
		btn6.onClick.AddListener(Race3Event);

		if(isRealVersion){
			btn1.gameObject.SetActive(false);
			btn2.gameObject.SetActive(false);
			btn4.gameObject.SetActive(false);
		}
		exitButton.onClick.AddListener(exitEvent);
		calibButton.onClick.AddListener(calibEvent);
		openGithub.onClick.AddListener(gitEvent);
		aboutSimBtn.onClick.AddListener(aboutEvent);
		terms.onClick.AddListener(termsEvent);
		back.onClick.AddListener(aboutBackEvent);
		
		if(isAlphaVersion){
			btn1.gameObject.SetActive(false);
			btn2.gameObject.SetActive(false);
			btn3.gameObject.SetActive(false);
			btn5.gameObject.SetActive(false);
			btn6.gameObject.SetActive(false);
		}
		// Texts
		// headerText = GameObject.Find("Canvas/Scroll View/Viewport/Content/Panel/HeaderTxt").GetComponent<Text>();
		// versionText = GameObject.Find("Canvas/Text").GetComponent<Text>();
		
		//Language Handler
		GameObject gameObject = new GameObject("LanguageHandler");
        languageHandler = gameObject.AddComponent<LanguageHandler>();
		languageHandler.m_dictionary();

		//Localization Settings
		Debug.Log(dropDown.options[dropDown.value].text);
		dropDown.onValueChanged.AddListener(delegate {
            languageChanged(dropDown);
        });

        string currentLang = PlayerPrefs.GetString("Language", "en");
		PlayerPrefs.SetString("Language", currentLang);
		if(currentLang == "fa"){
			currentLang = "Persian";
		}else if(currentLang == "en"){
			currentLang = "English";
		}else if(currentLang == "ru"){
			currentLang = "Russian";
		}else if(currentLang == "fr"){
			currentLang = "French";
		}else if(currentLang == "jp"){
			currentLang = "Japanese";
		}else if(currentLang == "cn"){
			currentLang = "Chinese";
		}else if(currentLang == "kr"){
			currentLang = "Korean";
		}else if(currentLang == "tu"){
			currentLang = "Turkish";
		}else if(currentLang == "de"){
			currentLang = "German";
		}else if(currentLang == "it"){
			currentLang = "Italian";
		}else if(currentLang == "sp"){
			currentLang = "Spanish";
		}

        
		int langIndex = dropDown.options.FindIndex((i) => { return i.text.Equals(currentLang);});
		Debug.Log(langIndex);
		dropDown.value = langIndex;	
	
		if(currentLang == "Persian" || currentLang == "English" || currentLang == "Russian"){
			translatedIcon.enabled = false;
		}else{
			translatedIcon.enabled = true;
		}

	
		//Set Language Texts
		UpdateTexts();
	}

	void UpdateTexts(){
		if(languageHandler.lang == "fa"){
			btn1.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["RaceTrack1"], false, false);
			btn2.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["RaceTrack2"], false, false);
			btn3.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["UrbanTrack1"], false, false);
			btn4.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["UrbanTrack2"], false, false);
			headerText.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["ChooseTrack"], false, false);
			calibButton.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["CameraCalibration"], false, false);
			terms.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["terms"], false, false);
			aboutSimBtn.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["aboutThis"], false, false);
			exitButton.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["Exit"], false, false);
			aboutHeaderText.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["About"], false, false);
		}else{
			btn1.GetComponentInChildren<Text>().text = languageHandler.dict["RaceTrack1"];
			btn2.GetComponentInChildren<Text>().text = languageHandler.dict["RaceTrack2"];
			btn3.GetComponentInChildren<Text>().text = languageHandler.dict["UrbanTrack1"];
			btn4.GetComponentInChildren<Text>().text = languageHandler.dict["UrbanTrack2"];
			terms.GetComponentInChildren<Text>().text = languageHandler.dict["terms"];
			aboutSimBtn.GetComponentInChildren<Text>().text = languageHandler.dict["aboutThis"];
			headerText.GetComponentInChildren<Text>().text = languageHandler.dict["ChooseTrack"];
			calibButton.GetComponentInChildren<Text>().text = languageHandler.dict["CameraCalibration"];
			exitButton.GetComponentInChildren<Text>().text = languageHandler.dict["Exit"];
			aboutHeaderText.GetComponentInChildren<Text>().text = languageHandler.dict["About"];
		}
		
		//Fianl version Changes
		if(isFinalVersion){
			versionResult = versionTextStr + finalVersion;
			btn2.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["RaceFinal"], false, false);
			btn4.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["UrbanFinal"], false, false);
		}else{
			versionResult = versionTextStr + version;
		}
		versionText.text = versionResult;
	}

	void languageChanged(Dropdown drop){
		string selectedLang = dropDown.options[dropDown.value].text;
		
		if(selectedLang == "Persian"){
			PlayerPrefs.SetString("Language", "fa");
			translatedIcon.enabled = false;
		}else if(selectedLang == "English"){
			PlayerPrefs.SetString("Language", "en");
			translatedIcon.enabled = false;
		}else if(selectedLang == "Russian"){
			PlayerPrefs.SetString("Language", "ru");
			translatedIcon.enabled = false;
		}else if(selectedLang == "French"){
			PlayerPrefs.SetString("Language", "fr");
			translatedIcon.enabled = true;
		}else if(selectedLang == "Japanese"){
			PlayerPrefs.SetString("Language", "jp");
			translatedIcon.enabled = true;
		}else if(selectedLang == "Chinese"){
			PlayerPrefs.SetString("Language", "cn");
			translatedIcon.enabled = true;
		}else if(selectedLang == "Korean"){
			PlayerPrefs.SetString("Language", "kr");
			translatedIcon.enabled = true;
		}else if(selectedLang == "Turkish"){
			PlayerPrefs.SetString("Language", "tu");
			translatedIcon.enabled = true;
		}else if(selectedLang == "German"){
			PlayerPrefs.SetString("Language", "de");
			translatedIcon.enabled = true;
		}else if(selectedLang == "Spanish"){
			PlayerPrefs.SetString("Language", "sp");
			translatedIcon.enabled = true;
		}else if(selectedLang == "Italian"){
			PlayerPrefs.SetString("Language", "it");
			translatedIcon.enabled = true;
		}
		languageHandler.m_dictionary();
		UpdateTexts();
	}
	void exitEvent()
    {
		Application.Quit();
	}
	void calibEvent()
    {
		SceneManager.LoadScene("Calibration");
	}
	void Race1Event()
	{
		SceneManager.LoadScene("Race1");
	}
	void Race2Event()
	{	
		if(isFinalVersion){
			SceneManager.LoadScene("Race2");
		}else{
			SceneManager.LoadScene("Race2");
		}
	}

	void gitEvent(){
		Application.OpenURL(githubURL);
	}
	void aboutEvent(){
		Show(canvasAbout);
	}
	void aboutBackEvent(){
		Hide(canvasAbout);
	}
	void termsEvent(){
		Application.OpenURL(termsURL);
	}
	void Urban1Event()
	{
		if(isRealVersion){
			SceneManager.LoadScene("RealUrban");

		}
		else{
			SceneManager.LoadScene("Urban1");
		}
	}
	void Urban2Event()
	{	
		if(isRealVersion){
			//SceneManager.LoadScene("RealUrban");
		}else{
			if(isFinalVersion){
				SceneManager.LoadScene("Urban3");
			}else{
				SceneManager.LoadScene("Urban2");
			}
		}
	}
	
	void Urban3Event()
	{
		SceneManager.LoadScene("Urban3");
	}

	void Race3Event()
	{
		SceneManager.LoadScene("Race3");
	}

	void Hide(Canvas canvas)
    {
        canvas.enabled = false;
     
    }
    void Show(Canvas canvas)
    {
        canvas.enabled = true;
    }
}
