using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject destroyedVersion;
    public GameObject ExplodeEffect;
    // Start is called before the first frame update
    public void DestroyObject(bool makeTrigger = false)
    {
        GameObject destroyedVer = Instantiate(destroyedVersion, transform.position, transform.rotation);
        if(makeTrigger){
            destroyedVer.transform.localScale /= 3;
        }
        Destroy(destroyedVer, 4);
        Destroy(gameObject);
    }
}
