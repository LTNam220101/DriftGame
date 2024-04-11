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
    IEnumerator Start()
    {
        TerrainController TerrainController = GetComponent<TerrainController>();
        int selectedCar = PlayerPrefs.GetInt("selectedCharacter");
        Vector3 startPoint = new Vector3(
            spanwPoint.transform.position.x,
            spanwPoint.transform.position.y,
            spanwPoint.transform.position.z
        );
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.point.y > TerrainController.Water.transform.position.y && hit.collider.CompareTag("Terrain")) {
                Vector3 newPosition = carPrefabs[selectedCar].transform.position;
                newPosition.y = hit.point.y + 2;
                carPrefabs[selectedCar].transform.position = newPosition;
                carPrefabs[selectedCar].SetActive(true);
                gameObject.GetComponent<TerrainController>().playerTransform = carPrefabs[selectedCar].transform;
                gameObject.GetComponent<Spawner>().Player = carPrefabs[selectedCar];
                cams[selectedCar].gameObject.SetActive(true); 
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.001f);
        }
    }
}
