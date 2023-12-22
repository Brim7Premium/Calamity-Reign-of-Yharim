using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private PlayerAI playerAI;
    private GameObject timerobj;

    private void Start()
    {
        playerAI = player.GetComponent<PlayerAI>();
        timerobj = GameObject.Find("RespawnTimer");
        timerobj.SetActive(false);
    }

    IEnumerator Respawn()
    {
		var settingsobj = GameObject.Find("[Settings]");
		if (settingsobj == null)
		{
			settingsobj = new GameObject { name = "[Settings]" };
			settingsobj.AddComponent<bracketSettingsbracket>();
			DontDestroyOnLoad(settingsobj);
		}
        var respawnSeconds = settingsobj.GetComponent<bracketSettingsbracket>().respawnTime;
        var seconds = respawnSeconds;
        timerobj.SetActive(true);
        while (0 != seconds)
        {
            timerobj.GetComponent<TextMeshProUGUI>().text = $"{seconds}";
            yield return new WaitForSeconds(1);
            seconds -= 1;
        }
        seconds = respawnSeconds;
        player.SetActive(true);
        timerobj.SetActive(false);
        playerAI.SetDefaults();
        playerAI.StartCoroutine(playerAI.Immunity());
    }
}
