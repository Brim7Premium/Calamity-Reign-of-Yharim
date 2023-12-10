using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private PlayerAI playerAI;

    [SerializeField] private int respawnSeconds = 5;

    private void Start()
    {
        playerAI = player.GetComponent<PlayerAI>();
    }

    IEnumerator Respawn()
    {
        var seconds = respawnSeconds;
        while (0 != seconds)
        {
            seconds -= 1;
            // you can show a respawn timer later
            yield return new WaitForSeconds(1);
        }
        player.SetActive(true);
        playerAI.SetDefaults();
        playerAI.StartCoroutine(playerAI.Immunity());
    }
}
