using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Player;
    public GameObject Cop;
    public TimerController Timer;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        TerrainController TerrainController = GetComponent<TerrainController>();
        while (true)
        {
            if(Player != null)
            {
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
                    GameObject cop = Instantiate(Cop, new Vector3(startPoint.x, hit.point.y + 2, startPoint.z), Quaternion.Euler(rotationVec.x, rotationVec.y, rotationVec.z));
                    cop.GetComponent<CopController>().Player = Player;
                    cop.GetComponent<CopController>().timer = Timer;
                }
            }
           yield return new WaitForSeconds(Random.value * 2);
        }
    }
}