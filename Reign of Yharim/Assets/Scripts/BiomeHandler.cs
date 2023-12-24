using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeHandler : MonoBehaviour
{
	public GameObject player;
	public GameObject worldManager;
	
	void Update()
	{
		if (player == null)
		{
			player = GameObject.Find("Player");
		}

		if (worldManager == null)
		{
			worldManager = GameObject.Find("WorldManager");
		}
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if (player != null && worldManager != null)
		{
			if (collision.gameObject.name == "Player")
			{
				worldManager.GetComponent<BiomeDetection>().biomeName = this.gameObject.name;
			}
		}
	}
}
