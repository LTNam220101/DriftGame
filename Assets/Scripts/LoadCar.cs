using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LoadCar : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public CinemachineVirtualCamera[] cams;
    public Transform spanwPoint;

    // Start is called before the first frame update
    void Start()
    {
        int selectedCar = PlayerPrefs.GetInt("selectedCharacter");
        carPrefabs[selectedCar].SetActive(true);
        gameObject.GetComponent<TerrainController>().playerTransform = carPrefabs[selectedCar].transform;
        gameObject.GetComponent<Spawner>().Player = carPrefabs[selectedCar];
        cams[selectedCar].gameObject.SetActive(true);
    }
}
