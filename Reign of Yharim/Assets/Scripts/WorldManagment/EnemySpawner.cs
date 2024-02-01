using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class EnemySpawner : MonoBehaviour
{
	public GameObject player;
	public List<NPC> enemiesToSpawn = new List<NPC>(); // this script must be added to the prefab for enemies you want to naturally spawn, as it sets their spawn chance and condition ect
	public float spawnRate = 5; // how often you want to try to spawn enemies

	public string parentBiome; // the biome this spawner uses, can be set manually for whatever reason

	public IEnumerator SpawnEnemies()
	{
		if (!BiomeManager.instance.bossAlive && (string.IsNullOrEmpty(parentBiome) || BiomeManager.instance.biomeName == parentBiome))
		{
			for (int i = 0; i < enemiesToSpawn.Count; ++i) // iterate through the list of enemies this object can spawn
			{
				if (UnityEngine.Random.value <= enemiesToSpawn[i].spawningParams.spawnChance && player != null) // if the spawn chance is met and the player is valid
				{
					if (string.IsNullOrEmpty(enemiesToSpawn[i].spawningParams.condition) || player.GetComponent<PlayerAI>().Plundered.Contains(enemiesToSpawn[i].spawningParams.condition)) // if theres no condition for the enemy or if youve met its condition
					{
						yield return SpawnEnemy(enemiesToSpawn[i].gameObject); // spawn the enemy
					}
					else
					{
						Debug.Log($"Failed to spawn {enemiesToSpawn[i].gameObject.name}");
					}
				}
			}
		}
	}

	IEnumerator Start()
	{	
		if (string.IsNullOrEmpty(parentBiome))
		{
			int index = gameObject.scene.name.IndexOf("_");
			if (index >= 0)
			{
				parentBiome = gameObject.scene.name.Substring(0, index); // gets the biome from the scene
			}
		}
		while (player == null)
		{
			player = GameObject.Find("Player");
		}
		while (true)
		{
			if (player != null) // if the player is valid
			{
				yield return SpawnEnemies(); // spawn enemies...
				yield return new WaitForSeconds(spawnRate); // with a cooldown
			}
		}
	}

	public IEnumerator SpawnEnemy(GameObject enemyPrefab, Vector2 SpawnPosition = new Vector2())
	{
		if (SpawnPosition == new Vector2()) // if you don't specify the spawn position
		{
			int ground = 1 << LayerMask.NameToLayer("Ground");

			SpawnPosition = new Vector2(UnityEngine.Random.Range(player.transform.position.x - 24f, player.transform.position.x + 24f), UnityEngine.Random.Range(player.transform.position.y - 12f, player.transform.position.y + 12f));          

			SpawnPosition = Physics2D.Raycast(SpawnPosition, Vector2.down, Mathf.Infinity, ground).point;
			
			// I can't make enemies not to spawn in blocks in general, but this will reduce chances of this happening by a lot
			while (Physics2D.Raycast(SpawnPosition, Vector2.down, 0, ground))
			{
				for(int i = 0; i<10; i++) // The game will not stop working when there is no space to spawn an enemy
				{
					if (Physics2D.Raycast(SpawnPosition, Vector2.down, 0, ground))
					{
						SpawnPosition = new Vector2(UnityEngine.Random.Range(player.transform.position.x - 24f, player.transform.position.x + 24f), UnityEngine.Random.Range(player.transform.position.y - 12f, player.transform.position.y + 12f));
						SpawnPosition = Physics2D.Raycast(SpawnPosition, Vector2.down, Mathf.Infinity, ground).point;
					}
					else break;
				}

				yield return new WaitForFixedUpdate();
			}
		}

		if (enemyPrefab != null) // if the prefab is actually valid
		{
			if (string.IsNullOrEmpty(parentBiome) || BiomeManager.instance.biomeName == parentBiome) // if youre in the spawner's biome
			{
				GameObject newEnemy = Instantiate(enemyPrefab, SpawnPosition, Quaternion.identity); // spawn the enemy
				if (newEnemy.scene != player.scene)
				{
					SceneManager.MoveGameObjectToScene(newEnemy, player.scene); // move it to systems so it wont despawn if it follows you too far out
				}
				newEnemy.SetActive(true); // enable the enmy
			}
		}
	}
}
