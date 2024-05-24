using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [Tooltip("The effect that is created at the location of this item when it is picked up")]
    public Transform pickupEffect;

    [Tooltip("A random rotation given to the object only on the Y axis")]
    public float randomRotation = 360;

    [Tooltip("rotationSpeed")]
    public float rotationSpeed = 30f;

    public GameObject controllerObj;
    public LoadCar loadCarComponent;
    public TimerController timerController;

    void Start()
    {
        // Set a random rotation angle for the object
        transform.eulerAngles += Vector3.up * Random.Range(-randomRotation, randomRotation);
        controllerObj = GameObject.Find("Controller");
        loadCarComponent = controllerObj.GetComponent<LoadCar>();
        timerController = controllerObj.GetComponent<TimerController>();
    }

    // Function to play the pickup sound
    void PlayPickupSound()
    {
        if (loadCarComponent.BuffMusic != null && loadCarComponent.BuffMusic.clip != null)
        {
            
            bool disableSound = PlayerPrefs.GetInt("disableSound") == 1 ? true : false;
            if(!disableSound) loadCarComponent.BuffMusic.Play();
        }
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
            if (loadCarComponent != null){
                int totalStar = PlayerPrefs.GetInt("totalStar", 0);
                PlayerPrefs.SetInt("totalStar", totalStar + 1);
            }
            timerController.AddStars();

            PlayPickupSound();
            // If there is a pick up effect, create it
            if (pickupEffect) Instantiate(pickupEffect, transform.position, transform.rotation);
            
            // Remove the object from the game
            Destroy(gameObject);
        }
    }
}
