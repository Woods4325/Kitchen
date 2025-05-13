using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StoveCounterVisual : MonoBehaviour {
    [SerializeField] private GameObject stoveOnGameObject, particlesGameObject;
    
    public void StoveOn(){
        stoveOnGameObject.SetActive(true);
        particlesGameObject.SetActive(true);
    }

    public void StoveOff(){
        stoveOnGameObject.SetActive(false);
        particlesGameObject.SetActive(false);
    }
}
