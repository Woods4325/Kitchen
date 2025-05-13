using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour {
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Start(){
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        iconTemplate.gameObject.SetActive(false);
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e){
        UpdateVisual();
    }

    private void UpdateVisual(){
        foreach(Transform child in transform){
            if(child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach(var kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()){
            var iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
            iconTransform.gameObject.SetActive(true);
        }
    }
}
