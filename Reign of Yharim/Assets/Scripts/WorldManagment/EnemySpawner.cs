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
	public List<SpawnableEnemy> enemiesToSpawn = new List<SpawnableEnemy>();

	public string parentBiome;

	public IEnumerator SpawnEnemies()
	{
		for (int i = 0; i < enemiesToSpawn.Count; ++i)
		{
			float randomNum = UnityEngine.Random.Range(1, enemiesToSpawn[i].spawnChance);
			if (randomNum == 1 && player)
			{
				if (string.IsNullOrEmpty(enemiesToSpawn[i].condition))
				{
					yield return SpawnEnemy(enemiesToSpawn[i].gameObject);
				}
				else if (player.GetComponent<PlayerAI>().defeatedBosses.Contains(enemiesToSpawn[i].condition))
				{
					yield return SpawnEnemy(enemiesToSpawn[i].gameObject);
				}
				else
				{
					Debug.Log($"Failed to spawn {enemiesToSpawn[i].gameObject.name}");
				}
			}
		}
	}

	IEnumerator Start()
	{	
		int index = this.gameObject.scene.name.IndexOf("_");
		if (this.gameObject.scene.name != "Systems" && index >= 0)
		{
  			parentBiome = this.gameObject.scene.name.Substring(0, index);
		}
		while (player == null)
		{
			player = GameObject.Find("Player");
		}
		while (true)
		{
			if (player != null) 
			{
				yield return SpawnEnemies();
				yield return new WaitForSeconds(1);
			}
		}
	}

	public IEnumerator SpawnEnemy(GameObject enemyPrefab)
	{
		int ground = 1 << LayerMask.NameToLayer("Ground");

		Vector2 SpawnPosition = new Vector2(UnityEngine.Random.Range(player.transform.position.x - 24f, player.transform.position.x + 24f), UnityEngine.Random.Range(player.transform.position.y - 12f, player.transform.position.y + 12f));          

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

		if (enemyPrefab != null)
		{
			GameObject newEnemy = Instantiate(enemyPrefab, SpawnPosition, Quaternion.identity);
			if (newEnemy.scene != player.scene)
			{
				SceneManager.MoveGameObjectToScene(newEnemy, player.scene);
			}
			newEnemy.SetActive(true);
		}
	}
}
