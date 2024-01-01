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
    public GameObject enemyToSpawn;

    public float EnemySpawnRate;
    IEnumerator Start()
    {
        int ground = 1 << LayerMask.NameToLayer("Ground");
        while(true)
        {
            yield return new WaitForSeconds(EnemySpawnRate);

            Vector2 SpawnPosition = new Vector2(Random.Range(player.transform.position.x - 24f, player.transform.position.x + 24f), Random.Range(player.transform.position.y - 12f, player.transform.position.y + 12f));          

            SpawnPosition = Physics2D.Raycast(SpawnPosition, Vector2.down, Mathf.Infinity, ground).point;
            
            //I can't make enemies not to spawn in blocks in general, but this will reduce chances of this happening by a lot
            while (Physics2D.Raycast(SpawnPosition, Vector2.down, 0, ground))
            {
                for(int i = 0; i<10; i++)//The game will not stop working when there is no space to spawn an enemy
                {
                    if(Physics2D.Raycast(SpawnPosition, Vector2.down, 0, ground))
                    {
                        SpawnPosition = new Vector2(Random.Range(player.transform.position.x - 24f, player.transform.position.x + 24f), Random.Range(player.transform.position.y - 12f, player.transform.position.y + 12f));
                        SpawnPosition = Physics2D.Raycast(SpawnPosition, Vector2.down, Mathf.Infinity, ground).point;
                    }
                    else break;
                }

                yield return new WaitForFixedUpdate();
            }

            GameObject newEnemy = Instantiate(enemyToSpawn, SpawnPosition, Quaternion.identity);
            // newEnemy.GetComponent("Script").healthBar = 
            if (newEnemy.scene != this.gameObject.scene)
            {
                SceneManager.MoveGameObjectToScene(newEnemy, this.gameObject.scene);
            }
            newEnemy.SetActive(true);
        }
    }
}
