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
        yield return new WaitForSeconds(respawnSeconds);
        player.SetActive(true);
        playerAI.enabled = true;
        playerAI.Invoke("SetDefaults", 0);
        playerAI.StartCoroutine(playerAI.Immunity());
    }
}
