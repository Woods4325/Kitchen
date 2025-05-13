using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialUI : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI moveUpKeyText;
    [SerializeField] private TextMeshProUGUI moveDownKeyText;
    [SerializeField] private TextMeshProUGUI moveLeftKeyText;
    [SerializeField] private TextMeshProUGUI moveRightKeyText;
    [SerializeField] private TextMeshProUGUI interactKeyText;
    [SerializeField] private TextMeshProUGUI interactAltKeyText;
    [SerializeField] private TextMeshProUGUI pauseKeyText;
    [SerializeField] private TextMeshProUGUI gamePadMoveKeyText;
    [SerializeField] private TextMeshProUGUI gamePadInteractKeyText;
    [SerializeField] private TextMeshProUGUI gamePadInteractAltKeyText;
    [SerializeField] private TextMeshProUGUI gamePadPauseKeyText;

    private void Start(){
        GameInput.Instance.OnBingdingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e){
        if(KitchenGameManager.Instance.IsCountdownToStart()){
            Hide();
        }
    }
    private void GameInput_OnBindingRebind(object sender, EventArgs e){
        UpdateVisual();
    }

    private void UpdateVisual(){
        moveUpKeyText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Move_Up);
        moveDownKeyText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Move_Down);
        moveLeftKeyText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Move_Left);
        moveRightKeyText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Move_Right);
        interactKeyText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Interact);
        interactAltKeyText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Interact_Alt);
        pauseKeyText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Pause);
        gamePadInteractKeyText.text = GameInput.Instance.GetBindingType(GameInput.Binding.GamePad_Interact);
        gamePadInteractAltKeyText.text = GameInput.Instance.GetBindingType(GameInput.Binding.GamePad_Interact_Alt);
        gamePadPauseKeyText.text = GameInput.Instance.GetBindingType(GameInput.Binding.GamePad_Pause);
    }

    private void Show(){
        gameObject.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
}
