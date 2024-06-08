using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Player;
    public GameObject Target;
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
                        GenerateEnemy(randId, 0);
                        enemiesCost-= copCosts[randId];
                    }else if(enemiesCost <= 0){
                        break;
                    }
                }
            }
           yield return new WaitForSeconds(Random.value * 2);
        }
    }

    private void GenerateEnemy(int id, int tryNum){
        if(tryNum == 5) return;
        float x = Random.value;
        if(x > 0.5f){
            x = Player.transform.position.x + Random.Range(55, 75);
        }else
        {
            x = Player.transform.position.x - Random.Range(55, 75);
        }
        float z = Random.value;
        if (z > 0.5f){
            z = Player.transform.position.z + Random.Range(55, 75);
        }else
        {
            z = Player.transform.position.z - Random.Range(55, 75);
        }
        RaycastHit hit;
        Vector3 startPoint = new Vector3(x, Player.transform.position.y + TerrainController.TerrainSize.y * 2, z);
        if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.collider.CompareTag("Terrain")) {
            Collider[] colliders = Physics.OverlapSphere(hit.transform.position, 10f);
            // Duyệt qua tất cả các Collider
            foreach (Collider collider in colliders)
            {
                // Kiểm tra xem Collider có tag là "Tree" không
                if (collider.CompareTag("Cop") || collider.CompareTag("Tree"))
                {   
                    GenerateEnemy(id, tryNum + 1);
                    return;
                }
            }
            Vector3 rotationVec =  Vector3.up * Random.Range(-180, 180);
            GameObject cop = Instantiate(cops[id], new Vector3(startPoint.x, hit.point.y + 1, startPoint.z), Quaternion.Euler(rotationVec.x, rotationVec.y, rotationVec.z));
            cop.GetComponent<CopController>().Player = Target;
            cop.GetComponent<CopController>().timer = Timer;
            return;
        }
    }
}