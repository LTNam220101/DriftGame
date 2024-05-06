using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cop")
        {
            Explode();
        } else if (other.gameObject.tag == "BigPlayer") {
            Explode(true, 1);
        }
    }
    void OnTriggerEnter(Collider  other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cop")
        {
            Explode();
        } else if (other.gameObject.tag == "BigPlayer") {
            Explode(true, 1);
        }
    }

    public void Explode(bool makeTrigger = false, int type = -1)
    {
        Destructable dest = gameObject.GetComponent<Destructable>();
        if(dest != null)
        {
            dest.DestroyObject(makeTrigger, type);
        }

        // var rbs = GetComponentsInChildren<Rigidbody>();
        // foreach (var rb in rbs)
        // {
        //     rb.AddExplosionForce(100000, transform.position + transform.forward * 10, 10);
        // }
    }
}
