using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashBarUI : MonoBehaviour{
    private const string IS_FLASH = "IsFlash";
    [SerializeField] private StoveCounter stoveCounter;
    private float showWarningAmount = .5f;
    private Animator animator;
    // private float warningSoundMax = 1f;
    // private float warningSoundTimer;

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void Start(){
        stoveCounter.OnProgressChanged += StoveCounter_OnProgresChanged;
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        Hide();
    }

    private void Update(){
        
    }

    private void StoveCounter_OnProgresChanged(object sender, IHasProgress.OnProgressChangedEventArgs e){
        if(stoveCounter.IsFried() && e.progressNormalized >= showWarningAmount){
            Show();
        } else {
            Hide();
        }
    }
    private void StoveCounter_OnStateChanged(object sender, EventArgs e){
        if(!stoveCounter.IsFried()){
            Hide();
        }
    }

    private void Show(){
        animator.SetBool(IS_FLASH, true);
    }

    private void Hide(){
        animator.SetBool(IS_FLASH, false);
    }

}
