using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text mapName;
    [SerializeField] private Image mapImage;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject lockedIcon;
    private int currentLevelIndex;

    public void DisplayMap(Map _map){
        mapName.text = _map.mapName;
        mapImage.sprite = _map.mapImage;
    }

    public void UpdateMap(Map _newMap)
    {
        mapName.text = _newMap.mapName;
        mapName.color = _newMap.nameColor;
        mapImage.sprite = _newMap.mapImage;

        bool mapUnlocked = PlayerPrefs.GetInt("mapUnlocked", 3) >= _newMap.levelIndex;

        if (mapUnlocked)
            mapImage.color = Color.white;
        else
            mapImage.color = Color.gray;

        playButton.interactable = mapUnlocked;
        lockedIcon.SetActive(!mapUnlocked);
        currentLevelIndex = _newMap.levelIndex + 2;
    }

    public void LoadLevel()
    {
		PlayerPrefs.SetInt("currentMapIndex", currentLevelIndex);
        SceneManager.LoadScene(currentLevelIndex);
    }
}
