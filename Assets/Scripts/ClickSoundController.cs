using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSoundController : MonoBehaviour
{
    private static ClickSoundController instance = null;
    public static ClickSoundController Instance { get { return instance; } }
    void Awake(){
        if(instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        }else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
