using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] cops;
    public int[] copCosts;
    public TimerController Timer;
    private TerrainController TerrainController;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        TerrainController = GetComponent<TerrainController>();
        while (true)
        {
            if(Player != null)
            {
                int enemiesCost = (int)Timer.elapsedTime / 40 + 1;
                while(enemiesCost > 0){
                    int randId = Random.Range(0, cops.Length);
                    if(enemiesCost - copCosts[randId] >= 0) {
                        GenerateEnemy(randId);
                        enemiesCost-= copCosts[randId];
                    }else if(enemiesCost <= 0){
                        break;
                    }
                }
            }
           yield return new WaitForSeconds(Random.value * 2);
        }
    }

    private void GenerateEnemy(int id){
        float x = Random.value;
        if(x > 0.5f){
            x = Player.transform.position.x + Random.Range(70, 80);
        }else
        {
            x = Player.transform.position.x - Random.Range(70, 80);
        }
        float z = Random.value;
        if (z > 0.5f){
            z = Player.transform.position.z + Random.Range(70, 80);
        }else
        {
            z = Player.transform.position.z - Random.Range(70, 80);
        }
        RaycastHit hit;
        Vector3 startPoint = new Vector3(x, Player.transform.position.y + TerrainController.TerrainSize.y * 2, z);
        if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.collider.CompareTag("Terrain")) {
            Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
            Vector3 rotationVec =  Vector3.up * Random.Range(-180, 180);
            GameObject cop = Instantiate(cops[id], new Vector3(startPoint.x, hit.point.y + 1, startPoint.z), Quaternion.Euler(rotationVec.x, rotationVec.y, rotationVec.z));
            cop.GetComponent<CopController>().Player = Player;
            cop.GetComponent<CopController>().timer = Timer;
        }
    }
}