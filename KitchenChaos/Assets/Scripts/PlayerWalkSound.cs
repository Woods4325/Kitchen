using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkSound : MonoBehaviour{
    private Player player;
    private float footStepTime = .1f;
    private float footStepTimeCounter;

    private void Awake(){
        footStepTimeCounter = footStepTime;
        player = GetComponent<Player>();
    }

    private void Update(){
        footStepTimeCounter -= Time.deltaTime;
        if(footStepTimeCounter <= 0){
            footStepTimeCounter = footStepTime;
            if(player.GetWalk()){
                float volume = 1f;
                SoundManager.Instance.PlayerFootStepsSound(player.transform.position, volume);
            }
        }
    }

}
