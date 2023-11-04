using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyToSpawn;

    public float regularSlimeSpawnRate;
    IEnumerator Start()
    {
        while(true)
        {
            yield return new WaitForSeconds(regularSlimeSpawnRate);

            bool isPositionChoosen = false;

            Vector2 spawnPosition = new Vector2(Random.Range(player.transform.position.x - 24f, player.transform.position.x + 24f), 
                                                Random.Range(player.transform.position.y - 12f, player.transform.position.y + 12f));

            while (!isPositionChoosen)
            {
                
                for(int i = 0; i<10; i++)//The game will not stop working when there is no space to spawn an enemy
                {
                    spawnPosition = new Vector2(Random.Range(player.transform.position.x - 24f, player.transform.position.x + 24f), 
                                                Random.Range(player.transform.position.y - 12f, player.transform.position.y + 12f));

                    Vector2 distanceToPlayer = (Vector2)player.transform.position - spawnPosition;

                    if(Physics2D.RaycastAll(spawnPosition, distanceToPlayer.normalized, distanceToPlayer.magnitude, 1<<8).Length % 2 == 1) continue;

                    spawnPosition = Physics2D.Raycast(spawnPosition, Vector2.down, Mathf.Infinity, 1<<8).point;

                    isPositionChoosen = true;

                    break;
                }

                yield return new WaitForFixedUpdate();
            }

            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition + Vector2.one, Quaternion.identity);

            newEnemy.SetActive(true);
        }
    }
}
