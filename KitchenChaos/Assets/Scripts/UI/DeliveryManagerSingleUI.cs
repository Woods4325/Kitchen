using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake(){
        iconTemplate.gameObject.SetActive(false);
    }


    public void SetSingleUI(RecipeSO recipeSO){
        recipeName.text = recipeSO.name;
        foreach(Transform icon in iconContainer){
            if(icon != iconTemplate){
                Destroy(icon.gameObject);
            }
        }
        foreach(var kitchenObjectSO in recipeSO.kitchenObjectSOList){
            var iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
