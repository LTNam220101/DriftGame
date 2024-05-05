using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
	public GameObject[] characters;
	int carUnlocked;
	public int selectedCharacter = 0;

	public void Start()
	{
		carUnlocked = (int)PlayerPrefs.GetFloat("record", 0);
		carUnlocked = carUnlocked/60 + 1;
		if(carUnlocked == 0) carUnlocked = 1;
		if(carUnlocked > 4) carUnlocked = 4;
	}

	public void NextCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter = (selectedCharacter + 1) % carUnlocked;
		characters[selectedCharacter].SetActive(true);
	}

	public void PreviousCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter--;
		if (selectedCharacter < 0)
		{
			selectedCharacter += carUnlocked;
		}
		characters[selectedCharacter].SetActive(true);
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
		SceneManager.LoadScene(2, LoadSceneMode.Single);
	}
	public void Back()
	{
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}
}
