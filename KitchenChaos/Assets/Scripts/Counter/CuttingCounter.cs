using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {
    public static event EventHandler OnAnyCut;
    new public static void ResetStaticData(){
        OnAnyCut = null;
    }
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    [SerializeField] private ProgressBarUI progressBarUI;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgressCounter;

    private void Start(){
        cuttingProgressCounter = 0;
    }
    public override void Interact(Player player){
        if(HasKitchenObject()){
            if(player.HasKitchenObject()){
                if(player.GetKitchenObject().TryGetPlate(out var plateKitchenObject)){
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
                        progressBarUI.Hide();
                    }
                }
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
                progressBarUI.Hide();
            }
        } else {
            if(player.HasKitchenObject()){
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())){
                    cuttingProgressCounter = 0;
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    var cuttingRecipeSO = GetCuttingRecipeSOFromInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = 1.0f * cuttingProgressCounter / cuttingRecipeSO.cuttingProgress
                    });
                    progressBarUI.Show();
                }
            } else {
                
            }
        }
    }

    public override void InteractAlternate(Player player){
        if(HasKitchenObject()){
            if(player.HasKitchenObject()){

            } else {
                // cut the object
                var cuttingRecipeSO = GetCuttingRecipeSOFromInput(GetKitchenObject().GetKitchenObjectSO());
                if(cuttingRecipeSO != null){
                    cuttingProgressCounter ++;
                    OnCut?.Invoke(this, EventArgs.Empty);
                    OnAnyCut?.Invoke(this, EventArgs.Empty);
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = 1.0f * cuttingProgressCounter / cuttingRecipeSO.cuttingProgress
                    });
                    if(cuttingProgressCounter >= cuttingRecipeSO.cuttingProgress){
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
                        progressBarUI.Hide();
                    }
                }
            }
        } else {
            if(player.HasKitchenObject()){

            } else {
                
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO){
        foreach(var cuttingRecipeSO in cuttingRecipeSOArray){
            if(cuttingRecipeSO.input == kitchenObjectSO){
                return true;
            }
        }
        return false;
    }

    private KitchenObjectSO GetOutputFromInput(KitchenObjectSO kitchenObjectSO){
        foreach(var cuttingRecipeSO in cuttingRecipeSOArray){
            if(cuttingRecipeSO.input == kitchenObjectSO){
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }
    private CuttingRecipeSO GetCuttingRecipeSOFromInput(KitchenObjectSO kitchenObjectSO){
        foreach(var cuttingRecipeSO in cuttingRecipeSOArray){
            if(cuttingRecipeSO.input == kitchenObjectSO){
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
