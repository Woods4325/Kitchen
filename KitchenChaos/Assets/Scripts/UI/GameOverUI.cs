using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI recipeDeliveredText;
    [SerializeField] private Button mainMenuButton;

    private void Awake(){
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    private void Start(){
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
    } 
    private void Update(){

    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e){
        if(KitchenGameManager.Instance.IsGameOver()){
            recipeDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipeAmount().ToString();
            Show();
        } else {
            Hide();
        }
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
        mainMenuButton.Select();
    }
}