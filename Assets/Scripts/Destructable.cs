using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject destroyedVersion;
    public GameObject ExplodeEffect;
    // Start is called before the first frame update
    public void DestroyObject(bool makeTrigger = false, int type = -1, Vector3 position = default(Vector3))
    {
        GameObject destroyedVer = Instantiate(destroyedVersion, transform.position, transform.rotation);
        var rbs = destroyedVer.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.AddExplosionForce(500000, position, 10, 3.0f);
        }
        Collider collider = destroyedVer.GetComponent<Collider>();
        if(makeTrigger){
            collider.isTrigger = true;
        }
        if(type == 0){
            destroyedVer.transform.localScale /= 3;
            GameObject body = destroyedVer.transform.Find("police_car").gameObject;
            body.tag = "SmallCop";
            body.GetComponent<Collider>().isTrigger = true;
        }
        else if (type == 1) {
            GameObject explodeEffect = Instantiate(ExplodeEffect, transform.position, transform.rotation);
            Rigidbody rb = destroyedVer.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Tác động lực nổ vào Rigidbody
                rb.AddExplosionForce(10, transform.position + transform.up*5 + transform.forward*5, 10);
                Destroy(explodeEffect, 2);
            }
        }
        else if (type == 2) {
            GameObject body = destroyedVer.transform.Find("police_car").gameObject;
            body.GetComponent<Collider>().isTrigger = true;
        }
        Destroy(gameObject);
        Destroy(destroyedVer, 4);
    }
}
