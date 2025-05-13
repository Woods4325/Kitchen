using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter {

    public override void Interact(Player player){
        if(HasKitchenObject()){
            if(player.HasKitchenObject()){
                if(player.GetKitchenObject().TryGetPlate(out var plateKitchenObject)){
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
                    }
                } else if(GetKitchenObject().TryGetPlate(out plateKitchenObject)){
                    if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())){
                        player.GetKitchenObject().DestroySelf();
                    }
                }
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        } else {
            if(player.HasKitchenObject()){
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else {
                
            }
        }
    }

}
