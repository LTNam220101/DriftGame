using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LoadCar : MonoBehaviour
{
    public GameObject[] carPrefabs;
	public CinemachineVirtualCamera cam1;
	public CinemachineVirtualCamera cam2;
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
                cam1.Follow = carPrefabs[selectedCar].transform;
                cam2.Follow = carPrefabs[selectedCar].transform;
                // cams[selectedCar].gameObject.SetActive(true); 
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.001f);
        }
    }

    /// <summary>
    /// Change the score and update it
    /// </summary>
    /// <param name="MakeFirstPersonView">MakeFirstPersonView</param>
    public void  MakeFirstPersonView() {
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
        Time.timeScale = 0.3f;

        // //Update the score text
        // if (scoreText) {
        //     scoreText.GetComponent<Text>().text = score.ToString();

        //     // Play the score object animation
        //     if (scoreText.GetComponent<Animation>()) scoreText.GetComponent<Animation>().Play();
        // }
    }
}
