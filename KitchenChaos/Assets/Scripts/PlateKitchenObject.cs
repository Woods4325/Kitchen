using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class PlateKitchenObject : KitchenObject {
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }
    [SerializeField] private List<KitchenObjectSO> vaildKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake(){
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO){
        if(!vaildKitchenObjectSOList.Contains(kitchenObjectSO)){
            return false;
        }
        if(kitchenObjectSOList.Contains(kitchenObjectSO)){
            return false;
        }
        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
            kitchenObjectSO = kitchenObjectSO
        });
        return true;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList(){
        return kitchenObjectSOList;
    }
}
