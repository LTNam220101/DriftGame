using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [Tooltip("The function that runs when this object is touched by the target")]
    public string touchFunction = "MakeFirstPersonView";

    [Tooltip("The parameter that will be passed with the function")]
    public float functionParameter = 100;

    [Tooltip("The tag of the target object that the function will play from")]
    public string functionTarget = "LoadCar";

    [Tooltip("The effect that is created at the location of this item when it is picked up")]
    public Transform pickupEffect;

    [Tooltip("A random rotation given to the object only on the Y axis")]
    public float randomRotation = 360;

    [Tooltip("rotationSpeed")]
    public float rotationSpeed = 30f;

    public GameObject controllerObj;
    public LoadCar loadCarComponent;

    void Start()
    {
        // Set a random rotation angle for the object
        transform.eulerAngles += Vector3.up * Random.Range(-randomRotation, randomRotation);
        controllerObj = GameObject.Find("Controller");
        loadCarComponent = controllerObj.GetComponent<LoadCar>();
    }

    void Update()
    {
        // transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Is executed when this obstacle touches another object with a trigger collider
    /// </summary>
    /// <param name="other"><see cref="Collider"/></param>
    void OnTriggerEnter(Collider other)
    {
        // Check if the object that was touched has the correct tag
        if (other.gameObject.tag == "Player")
        {
            // Check that we have a target tag and function name before running
            if (touchFunction != string.Empty)
            {
                if (loadCarComponent != null){
                    // Gọi phương thức touchFunction trên thành phần LoadCar
                    int randomInt = Random.Range(0, 5);
                    if(randomInt == 1){
                        loadCarComponent.GoSmall();
                    }else if (randomInt == 2) {
                        loadCarComponent.MakeFirstPersonView();
                    }else if (randomInt == 3){
                        loadCarComponent.Nuclear();
                    }else if (randomInt == 4){
                        loadCarComponent.Slomo();
                    } else {
                        loadCarComponent.GoBig();
                    }
                }
            }

            // If there is a pick up effect, create it
            if (pickupEffect) Instantiate(pickupEffect, transform.position, transform.rotation);
            
            // Remove the object from the game
            Destroy(gameObject);
        }
    }
}
