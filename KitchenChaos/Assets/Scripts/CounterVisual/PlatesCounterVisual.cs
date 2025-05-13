using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint, plateVisualPrefab;
    private Stack<GameObject> plateVisualGameObjectStack;


    private void Awake(){
        plateVisualGameObjectStack = new Stack<GameObject>();   
    }
    private void Start(){
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e){
        var plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float plateOffset = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffset * plateVisualGameObjectStack.Count, 0);
        plateVisualGameObjectStack.Push(plateVisualTransform.gameObject);
    }
    private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e){
        var plateVisual = plateVisualGameObjectStack.Pop();
        Destroy(plateVisual);
    }
}
