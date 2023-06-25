using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject destroyedVersion;
    // Start is called before the first frame update
    public void DestroyObject()
    {
        GameObject destroyedVer = Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(destroyedVer, 4);
    }
}
