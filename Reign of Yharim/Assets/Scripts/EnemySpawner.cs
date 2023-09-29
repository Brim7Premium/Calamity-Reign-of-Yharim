using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyToSpawn;

    public float regularSlimeSpawnRate = 45.0f;

    IEnumerator Start()
    {
        
        while(true)
        {
            
            yield return new WaitForSeconds(regularSlimeSpawnRate);

            Vector2 SpawnPosition = new Vector2(Random.Range(player.transform.position.x - 24f, player.transform.position.x + 24f), Random.Range(player.transform.position.y - 12f, player.transform.position.y + 12f));          

            while (!Physics2D.Raycast(SpawnPosition, Vector2.down, 1) || (Physics2D.OverlapBox(SpawnPosition, (Vector2)enemyToSpawn.transform.localScale, 0, 1<<8) != null))
            {
                for(int i = 0; i<10; i++)//The game will not stop working when there is no space to spawn an enemy
                {
                    if(!(!Physics2D.Raycast(SpawnPosition, Vector2.down, 1) || (Physics2D.OverlapBox(SpawnPosition, (Vector2)enemyToSpawn.transform.localScale, 0, 1<<8) != null)))
                    {
                        SpawnPosition = new Vector2(Random.Range(player.transform.position.x - 24f, player.transform.position.x + 24f), Random.Range(player.transform.position.y - 12f, player.transform.position.y + 12f));
                    }
                }

                yield return new WaitForFixedUpdate();
            }

            GameObject newEnemy = Instantiate(enemyToSpawn, SpawnPosition, Quaternion.identity);

            newEnemy.SetActive(true);
        }
    }
}
