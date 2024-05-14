using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
	public GameObject[] characters;
	public string[] listName = {
		"Sedan",
		"F1",
		"Bus", 
		"Prometheus"
	};
	public int selectedCharacter;
	private string[] listCondition = {
		"",
		"Survived 90 seconds", 
		"150 Car crashes in one game", 
		"Pick 20 powerups in one game"
	};
    [SerializeField] private Text carName;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject lockedIcon;
    [SerializeField] private Text record;
    [SerializeField] private Text unlockCondition;
    [SerializeField] private Text playText;
	private float recordTime;
	private int mostCrashed, mostPowerUpPicked;
	public void Start()
	{
		recordTime = PlayerPrefs.GetFloat("record", 0);
        TimeSpan timePlaying = TimeSpan.FromSeconds(recordTime);
		mostCrashed = PlayerPrefs.GetInt("mostCrashed", 0);
		mostPowerUpPicked = PlayerPrefs.GetInt("mostPowerUpPicked", 0);
		record.text = "Current Status\n" + 
					"Best time: " + timePlaying.ToString("mm':'ss'.'ff") + "\n" +
					"Most crashes: " + mostCrashed + "\n" +
					"Most powerups picked: " + mostPowerUpPicked + "\n";
		selectedCharacter = PlayerPrefs.GetInt("selectedCharacter", 0);
		characters[selectedCharacter].SetActive(true);
		checkIsUnlocked();
	}

	public void NextCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter = (selectedCharacter + 1) % 4;
		characters[selectedCharacter].SetActive(true);
		checkIsUnlocked();
	}

	public void PreviousCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter--;
		if (selectedCharacter < 0)
		{
			selectedCharacter += 4;
		}
		characters[selectedCharacter].SetActive(true);
		checkIsUnlocked();
	}

	void checkIsUnlocked(){
		bool carUnlock;
		switch(selectedCharacter){
			case 0:
				carUnlock = true;
				break;
			case 1:
				carUnlock = recordTime >= 90f;
				break;
			case 2:
				carUnlock = mostCrashed >= 150;
				break;
			case 3:
				carUnlock = mostPowerUpPicked >= 20;
				break;
			default:
				carUnlock = false;
				break;
		}
		carName.text = listName[selectedCharacter];
        playButton.interactable = carUnlock;
		playText.color = carUnlock ? Color.white : new Color( 0.7843137f , 0.7843137f , 0.7843137f , 0.5019608f  );
        lockedIcon.SetActive(!carUnlock);
		unlockCondition.text = listCondition[selectedCharacter];
		unlockCondition.gameObject.SetActive(!carUnlock);
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
		SceneManager.LoadScene(2);
	}
	public void Back()
	{
		SceneManager.LoadScene(0);
	}
}
