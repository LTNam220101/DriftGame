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
        while (true)
        {
            if(Player != null)
            {
                float x = Random.value;
                if(x > 0.5f){
                    x = Player.transform.position.x + Random.Range(35, 40);
                }else
                {
                    x = Player.transform.position.x - Random.Range(35, 40);
                }
                float z = Random.value;
                if (z > 0.5f){
                    z = Player.transform.position.z + Random.Range(35, 40);
                }else
                {
                    z = Player.transform.position.z - Random.Range(35, 40);
                }
                Vector3 randomSpawnPosition = new Vector3(x, Player.transform.position.y, z);
                Vector3 rotationVec = transform.forward;
                GameObject cop = Instantiate(Cop, randomSpawnPosition, Quaternion.Euler(rotationVec.x, Random.Range(0.0f, 360.0f), rotationVec.z));
                cop.GetComponent<CopController>().Player = Player;
                cop.GetComponent<CopController>().timer = Timer;
            }
           yield return new WaitForSeconds(Random.value * 2);
        }
    }
}