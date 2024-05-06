using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{

    public void Awake(){
        AudioSource Music = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();        
        bool isMute = PlayerPrefs.GetInt("isMute") == 1 ? true : false;
        Music.mute = isMute;
    }

    public void Play()
    {
	    SceneManager.LoadScene(1);
    }
}
