using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArabicSupport;

public class Configuration : MonoBehaviour
{
    public string Language = "en";
    
    void Start()
    {
        Language = PlayerPrefs.GetString("Language", "en");
        Debug.Log(Language);
    }
}
