using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour {
    public static OptionUI Instance;
    [SerializeField] private Button soundEffectButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamePadInteractButton;
    [SerializeField] private Button gamePadInteractAltButton;
    [SerializeField] private Button gamePadPauseButton;
    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamePadInteractText;
    [SerializeField] private TextMeshProUGUI gamePadInteractAltText;
    [SerializeField] private TextMeshProUGUI gamePadPauseText;
    [SerializeField] private Transform pressToRebindKeyTransform;
    private Action onCloseButtonAction;

    private void Awake(){
        Instance = this;


        soundEffectButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        closeButton.onClick.AddListener(() => {
            Hide();
            onCloseButtonAction();
        });

        moveUpButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Left);
        });
        moveRightButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Right);
        });
        interactButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Interact);
        });
        interactAltButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Interact_Alt);
        });
        pauseButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Pause);
        });
        gamePadInteractButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.GamePad_Interact);
        });
        gamePadInteractAltButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.GamePad_Interact_Alt);
        });
        gamePadPauseButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.GamePad_Pause);
        });
    }

    private void Start(){
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        UpdateVisual();
        Hide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e){
        Hide();
    }

    private void UpdateVisual(){
        soundEffectText.text = "Sound Effect : " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music : " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Interact_Alt);
        pauseText.text = GameInput.Instance.GetBindingType(GameInput.Binding.Pause);
        gamePadInteractText.text = GameInput.Instance.GetBindingType(GameInput.Binding.GamePad_Interact);
        gamePadInteractAltText.text = GameInput.Instance.GetBindingType(GameInput.Binding.GamePad_Interact_Alt);
        gamePadPauseText.text = GameInput.Instance.GetBindingType(GameInput.Binding.GamePad_Pause);
    }

    private void ShowPressToRebindKey(){
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey(){
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding){
        ShowPressToRebindKey();

        GameInput.Instance.RebindBinding(binding, () => {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }

    public void Show(Action onCloseButtonAction){
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
        soundEffectButton.Select();
    }

    public void Hide(){
        gameObject.SetActive(false);
    }
}
