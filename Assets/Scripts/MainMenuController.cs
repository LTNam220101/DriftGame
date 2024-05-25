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
    public AudioSource SirenSound;
    public AudioSource ClickSound;
    public AudioClip MenuMusic;
    public Text Record;
    public Image soundButton;
    public Image musicButton;

    public Sprite musicOn;
    public Sprite musicOff;
    public Sprite soundOn;
    public Sprite soundOff;

    private void Awake(){
        bool disableSound = PlayerPrefs.GetInt("disableSound") == 1 ? true : false;
        SirenSound.mute = disableSound;
        ClickSound.mute = disableSound;
        Music = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        if(musicButton) musicButton.sprite = Music.mute ? musicOff : musicOn;
        if(soundButton) soundButton.sprite = disableSound ? soundOff : soundOn;
        float record = PlayerPrefs.GetFloat("record", 0);
        TimeSpan timePlaying = TimeSpan.FromSeconds(record);
        string timePlayingStr = "Record: " + timePlaying.ToString("mm':'ss'.'ff");
        if(Record) Record.text = timePlayingStr;
    }

    public void InitScene(AudioClip clip) {
        LoadScene();
        Music = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        Music.clip = clip;
        Music.Play();
    }

    public void LoadScene() {
        bool disableSound = PlayerPrefs.GetInt("disableSound") == 1 ? true : false;
        SirenSound.mute = disableSound;
        ClickSound.mute = disableSound;
        Music = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        float record = PlayerPrefs.GetFloat("record", 0);
        bool disableMusic = PlayerPrefs.GetInt("disableMusic") == 1 ? true : false;
        Music.mute = disableMusic;
        SaveMusicOption();
        if(musicButton) musicButton.sprite = Music.mute ? musicOff : musicOn;
        if(soundButton) soundButton.sprite = disableSound ? soundOff : soundOn;
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
        musicButton.sprite = Music.mute ? musicOff : musicOn;
    }
    public void toggleSound(){
        SirenSound.mute = !SirenSound.mute;
        ClickSound.mute = !ClickSound.mute;
        bool disableSound = PlayerPrefs.GetInt("disableSound") == 1 ? true : false;
        PlayerPrefs.SetInt("disableSound", disableSound ? 0 : 1);
        soundButton.sprite = disableSound ? soundOn : soundOff;
    }

    public void SaveMusicOption() {
        PlayerPrefs.SetInt("disableMusic", Music.mute ? 1 : 0);
    }
}
