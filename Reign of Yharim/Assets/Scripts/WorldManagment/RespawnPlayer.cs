using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private PlayerAI playerAI;
    private GameObject timerobj;

    [SerializeField] private int respawnSeconds = 5;

    private void Start()
    {
        playerAI = player.GetComponent<PlayerAI>();
        timerobj = GameObject.Find("RespawnTimer");
        timerobj.SetActive(false);
    }

    IEnumerator Respawn()
    {
        var seconds = respawnSeconds;
        timerobj.SetActive(true);
        while (0 != seconds)
        {
            yield return new WaitForSeconds(1);
            seconds -= 1;
            timerobj.GetComponent<TextMeshProUGUI>().text = $"{seconds}";
        }
        seconds = respawnSeconds;
        player.SetActive(true);
        timerobj.SetActive(false);
        playerAI.SetDefaults();
        playerAI.StartCoroutine(playerAI.Immunity());
    }
}
