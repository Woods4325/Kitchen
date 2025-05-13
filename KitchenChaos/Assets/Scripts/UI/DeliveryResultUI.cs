using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour{
    private const string POPUP = "Popup";
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successedColor;
    [SerializeField] private Sprite successedSprite;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite failedSprite;
    private Animator animator;

    private void Awake(){
        animator = GetComponent<Animator>();
    }
    private void Start(){
        DeliveryManager.Instance.OnDeliverySuccessed += DeliveryManager_OnDeliverySuccessed;
        DeliveryManager.Instance.OnDeliveryFailed += DeliveryManager_OnDeliveryFailed;
        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnDeliveryFailed(object sender, EventArgs e){
        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messageText.text = "DELIVERY\nFAILED";
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
    }

    private void DeliveryManager_OnDeliverySuccessed(object sender, EventArgs e){
        backgroundImage.color = successedColor;
        iconImage.sprite = successedSprite;
        messageText.text = "DELIVERY\nSUCCESS";
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
    }
}
