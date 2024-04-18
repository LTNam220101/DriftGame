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
    public SpawnBuff spawnBuffController;
    private GameObject currentCar;
    private GameObject[] copColliders;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        TerrainController TerrainController = GetComponent<TerrainController>();
        spawnBuffController = GetComponent<SpawnBuff>();
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
                currentCar = carPrefabs[selectedCar];
                Vector3 newPosition = currentCar.transform.position;
                newPosition.y = hit.point.y + 2;
                currentCar.transform.position = newPosition;
                currentCar.SetActive(true);
                gameObject.GetComponent<TerrainController>().playerTransform = currentCar.transform;
                gameObject.GetComponent<Spawner>().Player = currentCar;
                cam1.Follow = currentCar.transform;
                cam2.Follow = currentCar.transform;
                cam2.LookAt = currentCar.transform;
                // cams[selectedCar].gameObject.SetActive(true); 
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.001f);
        }
    }

    /// <summary>
    // Swap View
    /// </summary>
    /// <param name="MakeFirstPersonView">MakeFirstPersonView</param>
    public void MakeFirstPersonView() {
        cam2.enabled = true;
        cam1.enabled = false;
        Time.timeScale = 0.5f;
        spawnBuffController.isSpawning = false;
        // Bắt đầu coroutine để chuyển lại sau 5 giây
        StartCoroutine(SwitchBackToThirdPersonView());
    }
    public void MakeThirdPersonView()
    {
        cam1.enabled = true;
        cam2.enabled = false;
        Time.timeScale = 1f;
        spawnBuffController.isSpawning = true;
    }
    IEnumerator SwitchBackToThirdPersonView()
    {
        // Chờ 5 giây
        yield return new WaitForSeconds(5f);

        // Chuyển từ cam2 về cam1
        MakeThirdPersonView();
    }

    /// <summary>
    //Go small
    /// </summary>
    /// <param name="GoSmall">GoSmall</param>
    public void GoSmall() {
        copColliders = GameObject.FindGameObjectsWithTag("Cop");
        // Duyệt qua tất cả các Collider
        foreach (GameObject cop in copColliders)
        {
            cop.transform.localScale /= 3;
            cop.tag = "SmallCop";
        }
    }
}
