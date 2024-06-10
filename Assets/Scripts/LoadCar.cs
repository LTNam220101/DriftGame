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
    private Collider[] colliders;
    private int selectedCar;

    public Slider waitSlider; // Slider để hiển thị thời gian đợi
    public Image fillSlider;
    public Text buffName;
    
    public AudioSource BuffMusic;
    public MainMenuController MainController;

    public int maxBuff = 6;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        MainController.LoadScene();
        TerrainController TerrainController = GetComponent<TerrainController>();
        spawnBuffController = GetComponent<SpawnBuff>();
        selectedCar = PlayerPrefs.GetInt("selectedCharacter");
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
                gameObject.GetComponent<Spawner>().Target = currentCar;
                cam1.Follow = currentCar.transform;
                cam2.Follow = currentCar.transform;
                cam2.LookAt = currentCar.transform;
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
        buffName.text = "Warp Drive";
        // Bắt đầu coroutine để chuyển lại sau 5 giây
        StartCoroutine(SwitchBackToThirdPersonView());
    }
    public void MakeThirdPersonView()
    {
        cam1.enabled = true;
        cam2.enabled = false;
        buffName.text = "";
        spawnBuffController.isSpawning = true;
    }
    public void RestoreTimeScale()
    {
        Time.timeScale = 1f;
        buffName.text = "";
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
        currentCar.layer = 9;
        spawnBuffController.isSpawning = false;
        colliders = Physics.OverlapSphere(currentCar.transform.position, 500f);
        // Duyệt qua tất cả các Collider
        foreach (Collider collider in colliders)
        {
            // Kiểm tra xem Collider có tag là "Tree" không
            if (collider.CompareTag("Tree"))
            {
                collider.isTrigger = true;
            }
        }
        buffName.text = "Giant";
        // Bắt đầu coroutine để chuyển lại sau 5 giây
        StartCoroutine(SwitchBackToNormalSize());
    }
    public void MakeNormalSize()
    {
        currentCar.transform.localScale /= 2;
        currentCar.GetComponent<Rigidbody>().mass = 1000f;
        currentCar.tag = "Player";
        currentCar.layer = 0;
        spawnBuffController.isSpawning = true;
        buffName.text = "";
        // Duyệt qua tất cả các Collider
        foreach (Collider collider in colliders)
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
        buffName.text = "Shrink 'em";
        copColliders = GameObject.FindGameObjectsWithTag("Cop");
        // Duyệt qua tất cả các Collider
        foreach (GameObject cop in copColliders)
        {
            cop.transform.localScale /= 3;
            cop.tag = "SmallCop";
            cop.layer = 8;
        }
        StartCoroutine(CheckTime());
    }

    IEnumerator CheckTime() {
        // Chờ 2 giây
        yield return new WaitForSeconds(1.75f);
        buffName.text = "";
    }

    /// <summary>
    //Nuclear
    /// </summary>
    /// <param name="Nuclear">Nuclear</param>
    public void Nuclear(GameObject explodeEffect, Transform transform){
        GameObject explodeEffectObj = Instantiate(explodeEffect, transform.position, transform.rotation);
        bool disableSound = PlayerPrefs.GetInt("disableSound") == 1 ? true : false;
        explodeEffect.GetComponent<AudioSource>().mute = disableSound;
        explodeEffectObj.SetActive(true);
        buffName.text = "Nuclear";
        StartCoroutine(NuclearExplode());
    }

    IEnumerator NuclearExplode() {
        // Chờ 2 giây
        yield return calculateTime(1.75f);
        buffName.text = "";
        colliders = Physics.OverlapSphere(currentCar.transform.position, 200f);
        // Duyệt qua tất cả các Collider
        foreach (Collider collider in colliders)
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
        buffName.text = "Slomo";
        StartCoroutine(SwitchBackToNormalTimeScale());
    }

    IEnumerator SwitchBackToNormalTimeScale()
    {
        // Chờ 3 giây
        yield return calculateTime(1.5f);
        RestoreTimeScale();
    }

    /// <summary>
    //Amok
    /// </summary>
    /// <param name="Amok">Amok</param>
    public void Amok(){
        CarController carController = currentCar.GetComponent<CarController>();
        carController.MaxSpeed *= 1.5f;
        carController.MinSpeed *= 1.5f;
        carController.AmokEffect.SetActive(true);
        buffName.text = "Amok";
        currentCar.tag = "BigPlayer";
        currentCar.layer = 9;
        spawnBuffController.isSpawning = false;
        StartCoroutine(SwitchBackToNormalSpeed());
    }

    public void MakeNormalSpeed()
    {
        CarController carController = currentCar.GetComponent<CarController>();
        carController.MaxSpeed /= 1.5f;
        carController.MinSpeed /= 1.5f;
        carController.AmokEffect.SetActive(false);
        buffName.text = "";
        currentCar.tag = "Player";
        currentCar.layer = 0;
        spawnBuffController.isSpawning = true;
    }

    IEnumerator SwitchBackToNormalSpeed()
    {
        // Chờ 5 giây
        yield return calculateTime(5f);
        MakeNormalSpeed();
    }

    /// <summary>
    //Decoy
    /// </summary>
    /// <param name="Decoy">Decoy</param>
    public void Decoy(){
        buffName.text = "Decoy";
        GameObject decoy = Instantiate(currentCar, currentCar.transform.position, currentCar.transform.rotation * Quaternion.Euler(0, 30f, 0));
        decoy.layer = 11;
        decoy.GetComponent<CarController>().canControl = false;
        gameObject.GetComponent<Spawner>().Target = decoy;
        GameObject[] copColliders = GameObject.FindGameObjectsWithTag("Cop");
        // Duyệt qua tất cả các Collider
        foreach (GameObject cop in copColliders)
        {
            if(cop.layer != 10){
                cop.GetComponent<CopController>().Player = decoy;
            }
        }
        spawnBuffController.isSpawning = false;
        StartCoroutine(SwitchBackToCurrentCar(decoy, copColliders));
    }

    public void RemoveDecoy(GameObject decoy, GameObject[] copColliders)
    {
        Destroy(decoy);
        buffName.text = "";
        spawnBuffController.isSpawning = true;
        gameObject.GetComponent<Spawner>().Target = currentCar;
        // Duyệt qua tất cả các Collider
        foreach (GameObject cop in copColliders)
        {
            if(cop.layer != 10){
                cop.GetComponent<CopController>().Player = currentCar;
            }
        }
    }

    IEnumerator SwitchBackToCurrentCar(GameObject decoy, GameObject[] copColliders)
    {
        // Chờ 7 giây
        yield return calculateTime(7f);
        RemoveDecoy(decoy, copColliders);
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

    public void activeBuff(GameObject explodeEffect, Transform transform) {
        int randomInt = Random.Range(0, maxBuff);
        // int randomInt = 5;
        switch(randomInt){
            case 0: 
                GoBig();
                break;
            case 1: 
                GoSmall();
                break;
            case 2: 
                MakeFirstPersonView();
                break;
            case 3: 
                Nuclear(explodeEffect, transform);
                break;
            case 4: 
                Slomo();
                break;
            case 5: 
                Amok();
                break;
            case 6: 
                Decoy();
                break;
            default: 
                GoSmall();
                break;
        }
    }
}