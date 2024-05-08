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
    public int carCrashes;
    public int buffPickup;
    public int stars;

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
        carCrashes = 0;
        buffPickup = 0;
        stars = 0;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;  
        float record = PlayerPrefs.GetFloat("record", 0);
        if(elapsedTime > record){
            PlayerPrefs.SetFloat("record", elapsedTime);
        }
		int mostCrashed = PlayerPrefs.GetInt("mostCrashed", 0);
        if(carCrashes > mostCrashed){
            PlayerPrefs.SetInt("mostCrashed", carCrashes);
        }
		int mostPowerUpPicked = PlayerPrefs.GetInt("mostPowerUpPicked", 0);
        if(buffPickup > mostPowerUpPicked){
            PlayerPrefs.SetInt("mostPowerUpPicked", buffPickup);
        }
        Invoke(nameof(RestartGame), 4);
    }
    void RestartGame(){
		int level = PlayerPrefs.GetInt("currentMapIndex", 1);
        SceneManager.LoadScene(level);
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            // if(elapsedTime > PlayerPrefs.GetFloat("record", 0) && (elapsedTime > 20f || elapsedTime > 30f || elapsedTime > 40f || elapsedTime > 50f)){
            //     unlock.text = "New Car Unlocked!!!";
            // }
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            timeCounter.text = "Time: " + timePlaying.ToString("mm':'ss'.'ff") + "\n" +
                        "Cars: " + carCrashes + "\n" +
                        "Picked: " + buffPickup + "\n" +
                        "Stars: " + stars + "\n";
            yield return null;
        }
    }

    public void AddCarCrashed(){
        carCrashes++;
    }

    public void AddBuffPicked(){
        buffPickup++;
    }

    public void AddStars(){
        stars++;
    }
}
