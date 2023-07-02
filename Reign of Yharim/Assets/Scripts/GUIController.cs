using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIController : MonoBehaviour
{
    public TMP_Text guiText;
    public Animator guiHeartAnimator;
    public PlayerAI playerAI;

    public string currentAnimationState;

    const string HeartNormal = "Heart_normal";
    const string HeartDeath = "Heart_death";
    const string HeartFull = "Heart_full";

    void Update()
    {
        guiText.text = GameTime.displayTime;
        if (playerAI.life == playerAI.lifeMax)
        {
            ChangeAnimationState(HeartFull);
        }
        if (playerAI.life != playerAI.lifeMax && playerAI.life != 0f)
        {
            ChangeAnimationState(HeartNormal);
        }
        if (playerAI.life == 0f)
        {
            ChangeAnimationState(HeartDeath);
        }
    }

    public void ChangeAnimationState(string newAnimationState)
    {
        if (currentAnimationState == newAnimationState) return; //if currentAnimationState equals newAnimationState, stop the method (prevents animations from interupting themselves)

        guiHeartAnimator.Play(newAnimationState); //play the newState animation

        currentAnimationState = newAnimationState; //set currentAnimationState to newAnimationState
    }
}
