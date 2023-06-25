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
            //yield return new WaitForSeconds(2);
            //SceneManager.LoadScene("Night");
        }
    }

    void Explode()
    {
        Destructable dest = gameObject.GetComponent<Destructable>();
        if(dest != null)
        {
            dest.DestroyObject();
        }

        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.AddExplosionForce(100000, transform.position + transform.forward * 10, 10);
        }
    }
}
