using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class StoveCounter : BaseCounter, IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnStateChanged;
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    [SerializeField] private StoveCounterVisual stoveCounterVisual;
    [SerializeField] private ProgressBarUI progressBarUI;
    [SerializeField] private AudioSource audioSource;
    private enum State {
        Idle, 
        Frying, 
        Fried, 
        Burned
    }
    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO nowFryingRecipeSO;
    private BurningRecipeSO nowBurningRecipeSO;

    private void Start(){
        burningTimer = 0;
        fryingTimer = 0;
        state = State.Idle;
    }

    private void Update(){
        if(HasKitchenObject()){
            switch(state){
                case State.Idle:

                    break;

                case State.Frying:
                    fryingTimer -= Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = 1.0f * Mathf.Clamp((nowFryingRecipeSO.fryingTime - fryingTimer) / nowFryingRecipeSO.fryingTime, 0f, 1f)
                    });
                    if(fryingTimer <= 0f){
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(nowFryingRecipeSO.output, this);
                        nowBurningRecipeSO = GetBurningRecipeSOFromInput(GetKitchenObject().GetKitchenObjectSO());
                        if(nowBurningRecipeSO != null){
                            burningTimer = nowBurningRecipeSO.burningTime;
                            state = State.Fried;
                            OnStateChanged?.Invoke(this, EventArgs.Empty);
                        } else {
                            Debug.Log("!");
                            audioSource.Stop();
                            progressBarUI.Hide();
                            state = State.Idle;
                            OnStateChanged?.Invoke(this, EventArgs.Empty);
                        }
                    }
                    break;

                case State.Fried:
                    burningTimer -= Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = 1.0f * Mathf.Clamp((nowBurningRecipeSO.burningTime - burningTimer) / nowBurningRecipeSO.burningTime, 0f, 1f)
                    });
                    if(burningTimer <= 0f){
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(nowBurningRecipeSO.output, this);
                        nowBurningRecipeSO = null;
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                        audioSource.Stop();
                        stoveCounterVisual.StoveOff();
                        progressBarUI.Hide();
                    }

                    break;
                
                case State.Burned:
                    break;
            }
            
        }
    }
    public override void Interact(Player player){
        if(HasKitchenObject()){
            if(player.HasKitchenObject()){
                if(player.GetKitchenObject().TryGetPlate(out var plateKitchenObject)){
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
                        nowFryingRecipeSO = null;
                        nowBurningRecipeSO = null;
                        fryingTimer = 0f;
                        burningTimer = 0f;
                        audioSource.Stop();
                        stoveCounterVisual.StoveOff();
                        progressBarUI.Hide();
                        state = State.Idle;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
                nowFryingRecipeSO = null;
                nowBurningRecipeSO = null;
                fryingTimer = 0f;
                burningTimer = 0f;
                audioSource.Stop();
                stoveCounterVisual.StoveOff();
                progressBarUI.Hide();
                state = State.Idle;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
            }
        } else {
            if(player.HasKitchenObject()){
                if(HasFryingRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())){
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    nowFryingRecipeSO = GetFryingRecipeSOFromInput(GetKitchenObject().GetKitchenObjectSO());
                    fryingTimer = nowFryingRecipeSO.fryingTime;
                    stoveCounterVisual.StoveOn();
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = 1.0f * Mathf.Clamp((nowFryingRecipeSO.fryingTime - fryingTimer) / nowFryingRecipeSO.fryingTime, 0f, 1f)
                    });
                    audioSource.Play();
                    progressBarUI.Show();
                } else if (HasBurningRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())){
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    state = State.Fried;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    nowBurningRecipeSO = GetBurningRecipeSOFromInput(GetKitchenObject().GetKitchenObjectSO());
                    burningTimer = nowBurningRecipeSO.burningTime;
                    stoveCounterVisual.StoveOn();
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = 1.0f * Mathf.Clamp((nowBurningRecipeSO.burningTime - burningTimer) / nowBurningRecipeSO.burningTime, 0f, 1f)
                    });
                    audioSource.Play();
                    progressBarUI.Show();
                }
            } else {
                
            }
        }
    }
    private bool HasFryingRecipeWithInput(KitchenObjectSO kitchenObjectSO){
        foreach(var fryingRecipeSO in fryingRecipeSOArray){
            if(fryingRecipeSO.input == kitchenObjectSO){
                return true;
            }
        }
        return false;
    }
    private bool HasBurningRecipeWithInput(KitchenObjectSO kitchenObjectSO){
        foreach(var burningRecipeSO in burningRecipeSOArray){
            if(burningRecipeSO.input == kitchenObjectSO){
                return true;
            }
        }
        return false;
    }

    private KitchenObjectSO GetOutputFromInput(KitchenObjectSO kitchenObjectSO){
        foreach(var fryingRecipeSO in fryingRecipeSOArray){
            if(fryingRecipeSO.input == kitchenObjectSO){
                return fryingRecipeSO.output;
            }
        }
        return null;
    }
    private FryingRecipeSO GetFryingRecipeSOFromInput(KitchenObjectSO kitchenObjectSO){
        foreach(var fryingRecipeSO in fryingRecipeSOArray){
            if(fryingRecipeSO.input == kitchenObjectSO){
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOFromInput(KitchenObjectSO kitchenObjectSO){
        foreach(var burningRecipeSO in burningRecipeSOArray){
            if(burningRecipeSO.input == kitchenObjectSO){
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried(){
        return state == State.Fried;
    }
}
