using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueStuff : MonoBehaviour
{
    public Image talker;
    public TMP_Text dialogue;
    private GameObject dialogueBox;

    private void Start()
    {
        dialogueBox = gameObject;
        dialogueBox.SetActive(false);
    }
    public void SetDialogue(string text, SpriteRenderer talkerSprite, float duration)
    {
        dialogueBox.SetActive(true);
        dialogue.text = text;
        talker.sprite = talkerSprite.sprite;
        talker.color = talkerSprite.color;
        StartCoroutine(DialogueWait(duration));
    }

    public IEnumerator DialogueWait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        dialogueBox.SetActive(false);
    }
}
