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
    
    public AudioClip PlaygroundMusic;

    // // Start is called before the first frame update
    // void Start()
    // {
	// 	mapUnlocked = (int)PlayerPrefs.GetInt("mapUnlocked", 1);
    // }


    private void Awake()
    {
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
        bool isMute = PlayerPrefs.GetInt("isMute") == 1 ? true : false;
        AudioSource Music = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        Music.clip = PlaygroundMusic;
        Music.mute = isMute;
        Music.Play();
		PlayerPrefs.SetInt("currentMapIndex", currentMapIndex + 3);
		SceneManager.LoadScene(currentMapIndex + 3, LoadSceneMode.Single);
	}
    public void Back()
	{
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}
}
