using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IKitchenObjectParent{
    public static Player Instance{ get; private set;}
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs{
        public BaseCounter selectedCounter;
    }

    public event EventHandler OnPickedSomething;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private bool isWalking;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake(){
        if(Instance != null){
            Debug.LogError("There exist multiple Player Instance");
        }
        Instance = this;
    }

    private void Start(){
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }
    private void Update(){
        HandleMovement();
        HandleInteraction();
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e){
        if(!KitchenGameManager.Instance.IsGamePlaying()) return ;
        if(selectedCounter != null){
            selectedCounter.Interact(this);
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e){
        if(!KitchenGameManager.Instance.IsGamePlaying()) return ;
        if(selectedCounter != null){
            selectedCounter.InteractAlternate(this);
        }
    }
    private void HandleInteraction(){
        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, interactDistance, countersLayerMask)){
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)){
                ChangeSelectedCounter(baseCounter);
            } else {
                ChangeSelectedCounter(null);
            }
        } else {
            ChangeSelectedCounter(null);
        }

    }

    private void ChangeSelectedCounter(BaseCounter selectedCounter){
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }

    private void HandleMovement(){
        Vector2 inputVector = gameInput.GetMoveNormalize();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float rotateSpeed = 10f;
        if(moveDir != Vector3.zero){
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
            isWalking = true;
        } else {
            isWalking = false;
        }

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .6f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if(canMove){
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        } else {
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if(canMove){
                transform.position += moveDirX * moveSpeed * Time.deltaTime;
            } else {
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if(canMove){
                    transform.position += moveDirZ * moveSpeed * Time.deltaTime;
                }
            }
        }

    }

    public bool GetWalk(){
        return isWalking;
    }

    public Transform GetKitchenObjectFollowTransform(){
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject){
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null){
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject(){
        return kitchenObject;
    }
    public void ClearKitchenObject(){
        kitchenObject = null;
    }
    public bool HasKitchenObject(){
        return kitchenObject != null;
    }

}
