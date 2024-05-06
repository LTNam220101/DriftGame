using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
	public GameObject[] characters;
	int carUnlocked;
	public int selectedCharacter;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject lockedIcon;

	public void Start()
	{
		carUnlocked = (int)PlayerPrefs.GetFloat("record", 0);
		selectedCharacter = PlayerPrefs.GetInt("selectedCharacter", 0);
		carUnlocked = carUnlocked/60 + 1;
		if(carUnlocked == 0) carUnlocked = 1;
		if(carUnlocked > 4) carUnlocked = 4;
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
		bool carUnlock = carUnlocked > selectedCharacter;
        playButton.interactable = carUnlock;
        lockedIcon.SetActive(!carUnlock);
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
