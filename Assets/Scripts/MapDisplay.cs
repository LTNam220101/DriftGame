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
    [SerializeField] private Text lockedText;
    [SerializeField] private Text totalStar;

    public void Start(){
        int totalStarRecord = PlayerPrefs.GetInt("totalStar", 0);
        totalStar.text = "totalStar: " + totalStarRecord;
    }

    public void DisplayMap(Map _map){
        mapName.text = _map.mapName;
        mapImage.sprite = _map.mapImage;
    }

    public void UpdateMap(Map _newMap)
    {
        mapName.text = _newMap.mapName;
        mapName.color = _newMap.nameColor;
        mapImage.sprite = _newMap.mapImage;

        int totalStar = PlayerPrefs.GetInt("totalStar", 0);
        bool mapUnlocked = totalStar >= _newMap.requiredStar;

        if (mapUnlocked)
            mapImage.color = Color.white;
        else
            mapImage.color = Color.gray;

        playButton.interactable = mapUnlocked;
        lockedIcon.SetActive(!mapUnlocked);
        lockedText.gameObject.SetActive(!mapUnlocked);
        lockedText.text = _newMap.requiredStar + " stars required";
    }
}
