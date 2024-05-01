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
    public Text MusicButton;
    public Text Record;

    // Start is called before the first frame update
    void Awake()
    {
        Music = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        float record = PlayerPrefs.GetFloat("record", 0);
        bool music = PlayerPrefs.GetInt("isMute") == 1 ? true : false;
        Music.mute = music;
        MusicButton.text = Music.mute ? "SOUND: OFF" : "SOUND: ON";
        TimeSpan timePlaying = TimeSpan.FromSeconds(record);
        string timePlayingStr = "Record: " + timePlaying.ToString("mm':'ss'.'ff");
        Record.text = timePlayingStr;
        /*string s = PlayerPrefs.GetString("autostart");
        // Nếu chuỗi string null hoặc rỗng thì sẽ tạo một data mới với các giá trị mặc định
        if (string.IsNullOrEmpty(s))
        {
            Time.timeScale = 0f;
        }else Play();*/
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
        Music.mute = true;
        SceneManager.LoadScene(1);
    }

    public void ToggleMute(){
        Music.mute = !Music.mute;
        SaveMusicOption();
        MusicButton.text = Music.mute ? "SOUND: OFF" : "SOUND: ON";
    }

    public void SaveMusicOption() {
        PlayerPrefs.SetInt("isMute", Music.mute ? 1 : 0);
    }
}
