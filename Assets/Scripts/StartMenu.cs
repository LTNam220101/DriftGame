using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{
    AudioSource Music;
    public AudioSource AudioSound;
    public Image sound;
    public Image music;

    public Sprite musicOn;
    public Sprite musicOff;
    public Sprite soundOn;
    public Sprite soundOff;
    
    public void Awake(){
        Music = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();        
        bool disableMusic = PlayerPrefs.GetInt("disableMusic") == 1 ? true : false;
        bool disableSound = PlayerPrefs.GetInt("disableSound") == 1 ? true : false;
        Music.mute = disableMusic;
        AudioSound.mute = disableSound;
        music.sprite = disableMusic ? musicOff : musicOn;
        sound.sprite = disableSound ? soundOff : soundOn;
    }

    public void Play()
    {
	    SceneManager.LoadScene(1);
    }
    public void toggleMusic(){
        Music.mute = !Music.mute;
        PlayerPrefs.SetInt("disableMusic", Music.mute ? 1 : 0);
        music.sprite = Music.mute ? musicOff : musicOn;
    }
    
    public void toggleSound(){
        AudioSound.mute = !AudioSound.mute;
        bool disableSound = PlayerPrefs.GetInt("disableSound") == 1 ? true : false;
        PlayerPrefs.SetInt("disableSound", disableSound ? 0 : 1);
        sound.sprite = disableSound ? soundOn : soundOff;
    }
}
