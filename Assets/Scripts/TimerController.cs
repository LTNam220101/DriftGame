using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    public Text timeCounter;
    public Text unlock;

    private TimeSpan timePlaying;
    private bool timerGoing;

    public float elapsedTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeCounter.text = "Time: 00:00.00";
        timerGoing = false;
    }

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;  
        float record = PlayerPrefs.GetFloat("record", 0);
        if(elapsedTime > record){
            PlayerPrefs.SetFloat("record", elapsedTime);
        }
        Invoke(nameof(RestartGame), 4);
    }
    void RestartGame(){
        SceneManager.LoadScene("City");  
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > PlayerPrefs.GetFloat("record", 0) && (elapsedTime > 20f || elapsedTime > 30f || elapsedTime > 40f || elapsedTime > 50f)){
                unlock.text = "New Car Unlocked!!!";
            }
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timePlayingStr;

            yield return null;
        }
    }
}
