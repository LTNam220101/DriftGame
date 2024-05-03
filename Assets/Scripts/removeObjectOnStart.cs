using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class removeObjectOnStart : MonoBehaviour
{
    bool canRun = true;
    void Update()
    {
        if(canRun){
            // Find all game objects with names containing "(14, 0, 15)"
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>()
                .Where(go => 
                        go.name.Contains("(14, 0, 14)") || go.name.Contains("(15, 0, 14)") 
                        || go.name.Contains("(14, 0, 15)") || go.name.Contains("(15, 0, 15)")
                        || go.name.Contains("(14, 0, 16)") || go.name.Contains("(15, 0, 16)")
                        || go.name.Contains("(14, 0, 17)") || go.name.Contains("(15, 0, 17)")
                        || go.name.Contains("(14, 0, 18)") || go.name.Contains("(15, 0, 18)"))
                .ToArray();
            if(gameObjects.Length > 0){
                // Print the names of the found game objects
                foreach (GameObject go in gameObjects)
                {
                    // Get the transform of the GameObject whose children you want to destroy
                    Transform parentTransform = go.transform;

                    // Loop through all children of the parent GameObject
                    for (int i = parentTransform.childCount - 1; i >= 0; i--)
                    {
                        // Get the child at index i
                        Transform child = parentTransform.GetChild(i);

                        // Destroy the child GameObject
                        Destroy(child.gameObject);
                    }
                }
                canRun = false;
            }
        }
    }
}
