using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable] 
public class DialogueData
{
	public string text;
	public Sprite talkerSprite;
	public float duration = 1f;
}

public class DialogueStuff : MonoBehaviour
{
	public Image talker;
	public TMP_Text dialogue;
	private GameObject dialogueBox;
	public bool isTalking;
	public List<DialogueData> queue = new List<DialogueData>();

	private void Start()
	{
		dialogueBox = gameObject;
		if (queue.Count == 0)
		{
			isTalking = false;
			dialogueBox.SetActive(false);
		}
		else
		{
			SetDialogue(queue[0]);
		}
	}
	public void SetDialogue(DialogueData dialogueData)
	{
		isTalking = true;
		dialogueBox.SetActive(true);
		dialogue.text = dialogueData.text;
		talker.sprite = dialogueData.talkerSprite;
		StartCoroutine(DialogueWait(dialogueData.duration));
	}
	public IEnumerator DialogueWait(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		queue.RemoveAt(0);
		if (queue.Count == 0)
		{
			isTalking = false;
			dialogueBox.SetActive(false);
		}
		else
		{
			SetDialogue(queue[0]);
		}
	}
	public void AddQueue(DialogueData dialoguedata)
	{
		queue.Add(dialoguedata);
		if (!isTalking)
		{
			SetDialogue(dialoguedata);
		}
	}
}
