using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject PauseButton;
    public AudioSource Music;
    public AudioClip MenuMusic;
    public Text MusicButton;
    public Text Record;

    private void Awake(){
        Music = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        if(MusicButton) MusicButton.text = Music.mute ? "SOUND: OFF" : "SOUND: ON";
    }

    public void InitScene(AudioClip clip) {
        Music = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        float record = PlayerPrefs.GetFloat("record", 0);
        bool isMute = PlayerPrefs.GetInt("isMute") == 1 ? true : false;
        Music.clip = clip;
        Music.mute = isMute;
        Music.Play();
        SaveMusicOption();
        if(MusicButton) MusicButton.text = Music.mute ? "SOUND: OFF" : "SOUND: ON";
        TimeSpan timePlaying = TimeSpan.FromSeconds(record);
        string timePlayingStr = "Record: " + timePlaying.ToString("mm':'ss'.'ff");
        if(Record) Record.text = timePlayingStr;
    }

    public void InitMenuScene(){
        InitScene(MenuMusic);
    }

    public void Play()
    {
        /*mainMenu.SetActive(false);
	    timer.BeginTimer();
        Time.timeScale = 1f;*/
	    SceneManager.LoadScene(1);
    }

    public void Pause()
    {
        mainMenu.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        mainMenu.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public void ReturnMenu()
    {
        InitMenuScene();
        SceneManager.LoadScene(2);
    }

    public void ToggleMute(){
        Music = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        Music.mute = !Music.mute;
        SaveMusicOption();
        MusicButton.text = Music.mute ? "SOUND: OFF" : "SOUND: ON";
    }

    public void SaveMusicOption() {
        PlayerPrefs.SetInt("isMute", Music.mute ? 1 : 0);
    }
}
