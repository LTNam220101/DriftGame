using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MapSelection : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] scriptableObjects;

    [Header ("Display Scripts")]
    [SerializeField] private MapDisplay mapDisplay;
	// public Image[] maps;
	// int mapUnlocked;
    private int currentMapIndex = 0;
    
    public string[] PlaygroundMusicPaths; // Paths to audio files in StreamingAssets
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

        StartCoroutine(LoadAndPlayRandomMusic(Music, disableMusic));

		PlayerPrefs.SetInt("currentMapIndex", currentMapIndex + 3);
		SceneManager.LoadScene(currentMapIndex + 3);
	}
    private IEnumerator LoadAndPlayRandomMusic(AudioSource music, bool disableMusic)
    {
        int randomIndex = Random.Range(0, PlaygroundMusicPaths.Length);
        string path = Path.Combine(Application.streamingAssetsPath, PlaygroundMusicPaths[randomIndex]);
        // Add "file://" prefix to the path
        // path = "file://" + path;
        // Debug.Log(path);
        // Use UnityWebRequest to load audio clip asynchronously
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError || www.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                AudioClip clip = UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(www);
                music.clip = clip;
                music.mute = disableMusic;
                music.Play();
            }
        }
    }
    public void Back()
	{
		SceneManager.LoadScene(1);
	}
}
