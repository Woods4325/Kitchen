using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeliveryManager : MonoBehaviour {
    public static DeliveryManager Instance {get; private set;}
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnDeliverySuccessed;
    public event EventHandler OnDeliveryFailed;
    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private float timeToSpawnRecipe = 5f;
    [SerializeField] private int waitingRecipeSOMax = 4;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeCounter; 
    private int successfulRecipeAmount;

    private void Awake(){
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
        spawnRecipeCounter = 0f;
    }

    private void Update(){
        if(KitchenGameManager.Instance.IsGamePlaying()){
            spawnRecipeCounter -= Time.deltaTime;
            if(spawnRecipeCounter <= 0f){
                spawnRecipeCounter = timeToSpawnRecipe + UnityEngine.Random.Range(-1f, 2f);
                if(waitingRecipeSOList.Count < waitingRecipeSOMax){
                    var waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                    waitingRecipeSOList.Add(waitingRecipeSO);

                    OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                    // Debug.Log($"New Recipe: {waitingRecipeSO.recipeName}");
                }
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject){
        for(var i = 0; i < waitingRecipeSOList.Count; i ++){
            var waitingRecipeSO = waitingRecipeSOList[i];
            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count){
                // Has the same number of ingredient in the recipe
                // Debug.Log($"Comparing to {waitingRecipeSO.recipeName}");
                bool isMatch = true;
                // try to find each ingredient in the recipe
                foreach(var recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList){
                    bool ingredientFound = false;
                    foreach(var plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()){
                        // Debug.Log($"Compare {plateKitchenObjectSO} and {recipeKitchenObjectSO} : {plateKitchenObjectSO == recipeKitchenObjectSO}");
                        if(plateKitchenObjectSO == recipeKitchenObjectSO){
                            // Debug.Log($"Found {plateKitchenObjectSO.name}!");
                            ingredientFound = true;
                            break;
                        }
                    }
                    if(!ingredientFound){
                        isMatch = false;
                        break;
                    }
                }
                if(isMatch){
                    // Debug.Log($"Delivery Corrected! {waitingRecipeSO.recipeName}");
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnDeliverySuccessed?.Invoke(this, EventArgs.Empty);
                    successfulRecipeAmount ++;
                    return ;
                }
            }
        }
        OnDeliveryFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList(){
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipeAmount(){
        return successfulRecipeAmount;
    }

}
