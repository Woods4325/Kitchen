using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounter : BaseCounter {
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private float timeToSpwanPlate = 4f;
    [SerializeField] private int plateSpwanAmounter = 4;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float timeToSpwanPlateCounter;
    private int plateSpwanAmounterCounter;

    private void Start(){
        timeToSpwanPlateCounter = timeToSpwanPlate;
    }

    private void Update(){
        if(KitchenGameManager.Instance.IsGamePlaying()){
            timeToSpwanPlateCounter += Time.deltaTime;
            if(timeToSpwanPlateCounter >= timeToSpwanPlate + UnityEngine.Random.Range(-1f, 2f)){
                timeToSpwanPlateCounter = 0;
                if(plateSpwanAmounterCounter < plateSpwanAmounter){
                    plateSpwanAmounterCounter ++;
                    OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public override void Interact(Player player){
        if(!player.HasKitchenObject()){
            if(plateSpwanAmounterCounter > 0){
                plateSpwanAmounterCounter --;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
