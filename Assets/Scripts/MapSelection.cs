using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelection : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] scriptableObjects;

    [Header ("Display Scripts")]
    [SerializeField] private MapDisplay mapDisplay;
	// public Image[] maps;
	// int mapUnlocked;
    private int currentMapIndex = 0;
    
    public AudioClip[] PlaygroundMusic;
    public AudioSource Sound;

    // // Start is called before the first frame update
    // void Start()
    // {
	// 	mapUnlocked = (int)PlayerPrefs.GetInt("mapUnlocked", 1);
    // }


    void Start()
    {
        bool disableSound = PlayerPrefs.GetInt("disableSound") == 1 ? true : false;
        Sound.mute = disableSound;
		currentMapIndex = PlayerPrefs.GetInt("currentMapIndex", 3) - 3;
        ChangeMap(0);
    }

    public void ChangeMap(int _index)
    {
        currentMapIndex += _index;
        if (currentMapIndex < 0) currentMapIndex = scriptableObjects.Length - 1;
        if (currentMapIndex > scriptableObjects.Length - 1) currentMapIndex = 0;
        
        if(mapDisplay != null) mapDisplay.UpdateMap((Map)scriptableObjects[currentMapIndex]);
    }

    public void NextMap()
	{
		ChangeMap(1);
	}

	public void PreviousMap()
	{
		ChangeMap(-1);
	}

	public void StartGame()
	{   
        bool disableMusic = PlayerPrefs.GetInt("disableMusic") == 1 ? true : false;
        AudioSource Music = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        Music.clip = PlaygroundMusic[Random.Range(0, PlaygroundMusic.Length)];
        Music.mute = disableMusic;
        Music.Play();
		PlayerPrefs.SetInt("currentMapIndex", currentMapIndex + 3);
        StartCoroutine(LoadYourAsyncScene(currentMapIndex + 3));
		// SceneManager.LoadScene(currentMapIndex + 3);
	}
    public void Back()
	{
		SceneManager.LoadScene(1);
	}
    IEnumerator LoadYourAsyncScene(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        // While the asynchronous scene loads, continue returning control to the main thread
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
