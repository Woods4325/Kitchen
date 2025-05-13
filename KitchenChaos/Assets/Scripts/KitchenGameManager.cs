using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour {
    public static KitchenGameManager Instance {get; private set;}
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;


    private enum State {
        WaitingToStart, 
        CountdownToStart, 
        GamePlaying, 
        GameOver,
    }


    [SerializeField] private float waitingToStartTimerMax = 1f;
    [SerializeField] private float countdownToStartTimerMax = 3f;
    [SerializeField] private float gamePlayingTimerMax = 10f;


    private float waitingToStartTimer;
    private float countdownToStartTimer;
    private float gamePlayingTimer;
    private State state;
    private bool isGamePaused = false;

    private void Awake(){
        Instance = this;
        state = State.WaitingToStart;
        waitingToStartTimer = waitingToStartTimerMax;
        countdownToStartTimer = countdownToStartTimerMax;
        gamePlayingTimer = gamePlayingTimerMax;
    }

    private void Start(){
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void Update(){
        switch(state){
            case State.WaitingToStart:
                // waitingToStartTimer -= Time.deltaTime;
                // if(waitingToStartTimer <= 0f){
                //     state = State.CountdownToStart;
                //     OnStateChanged?.Invoke(this, EventArgs.Empty);
                // }
                break;

            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if(countdownToStartTimer <= 0f){
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if(gamePlayingTimer <= 0){
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:

                break;
        }
        // Debug.Log(state);
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e){
        TogglePausedGame();
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e){
        if(state == State.WaitingToStart){
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void TogglePausedGame(){
        if(isGamePaused){
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        isGamePaused ^= true;
    }

    public bool IsCountdownToStart(){
        return state == State.CountdownToStart;
    }
    public float GetCountdownToStartTimer(){
        return countdownToStartTimer;
    }

    public bool IsGamePlaying(){
        return state == State.GamePlaying;
    }

    public float GetGamePlayingTimerNormalized(){
        return gamePlayingTimer / gamePlayingTimerMax;
    }

    public bool IsGameOver(){
        return state == State.GameOver;
    }

}
