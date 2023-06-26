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
    public void Respawn()
    {
        StartCoroutine(RespawnLoop());
    }

    IEnumerator RespawnLoop()
    {
        yield return new WaitForSeconds(respawnSeconds);
        playerAI.active = true;
        playerAI.Invoke("SetDefaults", 0);
        player.SetActive(true);
        playerAI.StartCoroutine(playerAI.Immunity());
    }
}
