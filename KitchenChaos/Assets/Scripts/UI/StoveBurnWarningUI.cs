using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour{
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
        // warningSoundTimer -= warningSoundMax;
        // if(warningSoundTimer <= 0){
        //     warningSoundTimer = warningSoundMax;
        //     SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
        // }
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
        gameObject.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
}
