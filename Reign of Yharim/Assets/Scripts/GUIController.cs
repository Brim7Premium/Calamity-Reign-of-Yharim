using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIController : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text healthText;
    public Animator guiHeartAnimator;
    public PlayerAI playerAI;

    public string currentAnimationState;

    const string HeartNormal = "Heart_normal";
    const string HeartDeath = "Heart_death";
    const string HeartFull = "Heart_full";

    void Update()
    {
        timeText.text = GameTime.displayTime;

        if (playerAI.life >= 0)
            healthText.text = "Health: " + playerAI.life + "/" + playerAI.lifeMax;
        else
            healthText.text = "Health: 0/" + playerAI.lifeMax;

        if (playerAI.life == playerAI.lifeMax)
        {
            ChangeAnimationState(HeartFull);
        }
        if (playerAI.life != playerAI.lifeMax && playerAI.life > 0f)
        {
            ChangeAnimationState(HeartNormal);
        }
        if (playerAI.life <= 0f)
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
