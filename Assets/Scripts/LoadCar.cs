using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class LoadCar : MonoBehaviour
{
    public GameObject[] carPrefabs;
	public CinemachineVirtualCamera cam1;
	public CinemachineVirtualCamera cam2;
    public Transform spawnPoint;
    public SpawnBuff spawnBuffController;
    private GameObject currentCar;
    private GameObject[] copColliders;
    private Collider[] treeColliders;

    public Slider waitSlider; // Slider để hiển thị thời gian đợi
    public Image fillSlider; 
    
    public AudioSource BuffMusic;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        TerrainController TerrainController = GetComponent<TerrainController>();
        spawnBuffController = GetComponent<SpawnBuff>();
        int selectedCar = PlayerPrefs.GetInt("selectedCharacter");
        Vector3 startPoint = new Vector3(
            spawnPoint.transform.position.x,
            spawnPoint.transform.position.y,
            spawnPoint.transform.position.z
        );
        Time.timeScale = 1f;
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.collider.CompareTag("Terrain")) {
                currentCar = carPrefabs[selectedCar];
                Vector3 newPosition = currentCar.transform.position;
                newPosition.y = hit.point.y + 1;
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
        spawnBuffController.isSpawning = true;
    }
    public void RestoreTimeScale()
    {
        Time.timeScale = 1f;
    }
    IEnumerator SwitchBackToThirdPersonView()
    {
        // Chờ 5 giây
        
        yield return calculateTime(5f);
        // Chuyển từ cam2 về cam1
        MakeThirdPersonView();
        
        // Chờ 1 giây
        yield return new WaitForSeconds(1f);
        RestoreTimeScale();
    }

    /// <summary>
    // Go big
    /// </summary>
    /// <param name="GoBig">GoBig</param>
    public void GoBig() {
        currentCar.transform.localScale *= 2;
        currentCar.GetComponent<Rigidbody>().mass = 10000f;
        currentCar.tag = "BigPlayer";
        spawnBuffController.isSpawning = false;
        treeColliders = Physics.OverlapSphere(currentCar.transform.position, 500f);
        // Duyệt qua tất cả các Collider
        foreach (Collider collider in treeColliders)
        {
            // Kiểm tra xem Collider có tag là "Tree" không
            if (collider.CompareTag("Tree"))
            {
                collider.isTrigger = true;
            }
        }
        // Bắt đầu coroutine để chuyển lại sau 5 giây
        StartCoroutine(SwitchBackToNormalSize());
    }
    public void MakeNormalSize()
    {
        currentCar.transform.localScale /= 2;
        currentCar.GetComponent<Rigidbody>().mass = 1000f;
        currentCar.tag = "Player";
        spawnBuffController.isSpawning = true;
        // Duyệt qua tất cả các Collider
        foreach (Collider collider in treeColliders)
        {
            // Kiểm tra xem Collider có tag là "Tree" không
            if (collider!=null && collider.CompareTag("Tree"))
            {
                collider.isTrigger = false;
            }
        }
    }
    IEnumerator SwitchBackToNormalSize()
    {
        // Chờ 7 giây
        
        yield return calculateTime(7f);

        // Chuyển từ cam2 về cam1
        MakeNormalSize();
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

    /// <summary>
    //Nuclear
    /// </summary>
    /// <param name="Nuclear">Nuclear</param>
    public void Nuclear(){
        StartCoroutine(NuclearExplode());
    }

    IEnumerator NuclearExplode() {
        // Chờ 2 giây
        yield return calculateTime(1.75f);
        treeColliders = Physics.OverlapSphere(currentCar.transform.position, 200f);
        // Duyệt qua tất cả các Collider
        foreach (Collider collider in treeColliders)
        {
            // Kiểm tra xem Collider có tag là "Tree" không
            if (collider.CompareTag("Cop") || collider.CompareTag("SmallCop"))
            {   
                CopController controller= collider.GetComponent<CopController>();
                if (controller != null)
                {
                    // Gọi phương thức Explode()
                    controller.Explode();
                }
            }
        }
    }

    /// <summary>
    //Slomo
    /// </summary>
    /// <param name="Slomo">Slomo</param>
    public void Slomo(){
        Time.timeScale = 0.5f;
        StartCoroutine(SwitchBackToNormalTimeScale());
    }

    IEnumerator SwitchBackToNormalTimeScale()
    {
        // Chờ 3 giây
        yield return calculateTime(1.5f);
        RestoreTimeScale();
    }

    IEnumerator calculateTime(float waitTime) {
        waitSlider.gameObject.SetActive(true);
        float elapsedTime = 0f; // Thời gian đã trôi qua

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            // Cập nhật giá trị của Slider từ 0 đến 1 dựa trên thời gian đã trôi qua và thời gian chờ mong muốn
            waitSlider.value = elapsedTime / waitTime;
            if(elapsedTime / waitTime >= 0.9f) {
                fillSlider.color = Color.red;
            }else {
                fillSlider.color = Color.white;
            }
            yield return null; // Chờ một frame
        }
        waitSlider.gameObject.SetActive(false);
    }
}
